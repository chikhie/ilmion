using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Stripe;
using Stripe.Checkout;
using KitabStock.Infra;
using KitabStock.Infra.Payment;
using KitabStock.Infra.Entities;
using KitabStock.Infra.Services;
using KitabStock.Api.Interfaces;
using KitabStock.Infra.Mail;

namespace KitabStock.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeWebhookController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly StripePaymentService _stripeService;
    private readonly IMailService _mailService;
    private readonly ILogger<StripeWebhookController> _logger;

    public StripeWebhookController(
        ApplicationDbContext context,
        UserManager<UserEntity> userManager,
        StripePaymentService stripeService,
        IMailService mailService,
        ILogger<StripeWebhookController> logger)
    {
        _context = context;
        _userManager = userManager;
        _stripeService = stripeService;
        _mailService = mailService;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint webhook pour recevoir les événements Stripe
    /// </summary>
    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeSignature = Request.Headers["Stripe-Signature"].ToString();

        try
        {
            var stripeEvent = _stripeService.ConstructWebhookEvent(json, stripeSignature);
            
            _logger.LogInformation($"Webhook reçu: {stripeEvent.Type}");

            // Traiter l'événement selon son type
            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    await HandleCheckoutSessionCompleted(stripeEvent);
                    break;

                case "checkout.session.expired":
                    await HandleCheckoutSessionExpired(stripeEvent);
                    break;

                case "payment_intent.succeeded":
                    _logger.LogInformation("PaymentIntent succeeded");
                    break;

                case "payment_intent.payment_failed":
                    await HandlePaymentFailed(stripeEvent);
                    break;

                case "charge.refunded":
                    await HandleRefund(stripeEvent);
                    break;

                default:
                    _logger.LogInformation($"Événement non géré: {stripeEvent.Type}");
                    break;
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erreur webhook: {ex.Message}");
            return BadRequest($"Erreur: {ex.Message}");
        }
    }

    private async Task HandleCheckoutSessionCompleted(Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;
        if (session == null) return;

        _logger.LogInformation($"Session complétée: {session.Id}");

        // Récupérer les métadonnées
        var videoId = Guid.Parse(session.Metadata["video_id"]);
        var purchaseType = session.Metadata["purchase_type"];

        // Récupérer la vidéo
        var video = await _context.Videos.FindAsync(videoId);
        if (video == null)
        {
            _logger.LogError($"Vidéo non trouvée: {videoId}");
            return;
        }

        // Récupérer le PaymentIntent pour obtenir l'ID de transaction
        var paymentIntentId = session.PaymentIntentId;

        if (purchaseType == "authenticated")
        {
            // Achat avec compte utilisateur
            var userId = session.Metadata["user_id"];

            // Vérifier si l'achat n'existe pas déjà
            var existingPurchase = await _context.VideoPurchases
                .FirstOrDefaultAsync(vp => vp.UserId == userId && vp.VideoId == videoId);

            if (existingPurchase == null)
            {
                var purchase = new VideoPurchaseEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    VideoId = videoId,
                    PurchaseCode = GeneratePurchaseCode(),
                    PricePaid = video.Price,
                    PurchaseDate = DateTime.UtcNow,
                    Status = PurchaseStatus.Completed,
                    StripePaymentIntentId = paymentIntentId
                };

                _context.VideoPurchases.Add(purchase);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Achat créé pour l'utilisateur {userId}, vidéo {videoId}");
                
                // Envoyer un email de confirmation (optionnel)
                await SendPurchaseConfirmationEmail(session.CustomerEmail, video.Title, false, null);
            }
        }
        else if (purchaseType == "guest")
        {
            // Achat sans compte (invité)
            var guestEmail = session.Metadata["guest_email"];

            // Générer un code d'achat unique
            string purchaseCode;
            int attempts = 0;
            do
            {
                purchaseCode = PurchaseCodeGenerator.GenerateCode();
                attempts++;
                
                if (attempts > 10)
                {
                    _logger.LogError("Impossible de générer un code unique");
                    return;
                }
            }
            while (await _context.VideoPurchases.AnyAsync(vp => vp.PurchaseCode == purchaseCode));

            // Pour un achat invité, créer un user temporaire avec l'email
            var email = guestEmail;
            
            // Créer un user temporaire si nécessaire
            var user = await _userManager.FindByEmailAsync(email ?? "");
            if (user == null && !string.IsNullOrEmpty(email))
            {
                user = new UserEntity
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = false
                };
                await _userManager.CreateAsync(user);
            }
            
            if (user == null)
            {
                _logger.LogError("Impossible de créer un user pour l'achat invité");
                return;
            }

            var purchase = new VideoPurchaseEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                VideoId = videoId,
                PurchaseCode = purchaseCode,
                PricePaid = video.Price,
                PurchaseDate = DateTime.UtcNow,
                Status = PurchaseStatus.Completed,
                StripePaymentIntentId = paymentIntentId
            };

            _context.VideoPurchases.Add(purchase);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Achat invité créé, code: {purchaseCode}");

            // Envoyer l'email avec le code d'achat
            await SendPurchaseConfirmationEmail(guestEmail, video.Title, true, purchaseCode);
        }
    }

    private async Task HandleCheckoutSessionExpired(Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;
        if (session == null) return;

        _logger.LogInformation($"Session expirée: {session.Id}");
        // Gérer l'expiration si nécessaire (par exemple, nettoyer des données temporaires)
    }

    private async Task HandlePaymentFailed(Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        _logger.LogWarning($"Paiement échoué: {paymentIntent.Id}");
        // Gérer l'échec du paiement (notifications, logs, etc.)
    }

    private async Task HandleRefund(Event stripeEvent)
    {
        var charge = stripeEvent.Data.Object as Charge;
        if (charge == null) return;

        _logger.LogInformation($"Remboursement: {charge.Id}");

        // Trouver l'achat correspondant et mettre à jour son statut
        var purchase = await _context.VideoPurchases
            .FirstOrDefaultAsync(vp => vp.StripePaymentIntentId == charge.PaymentIntentId);

        if (purchase != null)
        {
            purchase.Status = PurchaseStatus.Refunded;
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Achat {purchase.Id} marqué comme remboursé");
        }
    }

    private async Task SendPurchaseConfirmationEmail(string? email, string videoTitle, bool isGuest, string? purchaseCode)
    {
        if (string.IsNullOrEmpty(email)) return;

        try
        {
            if (isGuest && !string.IsNullOrEmpty(purchaseCode))
            {
                // Email avec code d'achat (pour les invités)
                var subject = "Votre achat a été confirmé - Kitab";
                var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background: white; padding: 30px; border-radius: 10px; }}
        .header {{ background: linear-gradient(135deg, #2d3748 0%, #1a202c 100%); color: #d4af7a; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .code-box {{ background: #f7fafc; border: 2px dashed #d4af7a; padding: 20px; margin: 20px 0; text-align: center; font-size: 24px; font-weight: bold; letter-spacing: 2px; color: #2d3748; }}
        .content {{ padding: 20px; color: #2d3748; }}
        .footer {{ text-align: center; color: #718096; font-size: 12px; margin-top: 30px; }}
        .success {{ background: #48bb78; color: white; padding: 10px; border-radius: 5px; text-align: center; margin-bottom: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Kitab</h1>
            <p>Paiement confirmé ✓</p>
        </div>
        <div class='success'>
            ✓ Votre paiement a été traité avec succès
        </div>
        <div class='content'>
            <h2>Merci pour votre achat !</h2>
            <p>Vous avez acheté : <strong>{videoTitle}</strong></p>
            <p>Voici votre code d'achat unique :</p>
            <div class='code-box'>{purchaseCode}</div>
            <p><strong>Comment accéder à votre vidéo :</strong></p>
            <ol>
                <li>Allez sur Kitab</li>
                <li>Cliquez sur ""Accéder avec un code""</li>
                <li>Entrez votre code : <strong>{purchaseCode}</strong></li>
            </ol>
            <p style='color: #718096; font-size: 14px; margin-top: 20px;'>
                Conservez ce code précieusement. Vous en aurez besoin pour accéder à votre vidéo.
            </p>
        </div>
        <div class='footer'>
            <p>© 2025 Kitab. Tous droits réservés.</p>
            <p style='margin-top: 10px;'>Paiement sécurisé par Stripe</p>
        </div>
    </div>
</body>
</html>";

                await _mailService.SendEmailAsync(email, subject, body);
            }
            else
            {
                // Email de confirmation simple (pour les utilisateurs connectés)
                var subject = "Achat confirmé - Kitab";
                var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background: white; padding: 30px; border-radius: 10px; }}
        .header {{ background: linear-gradient(135deg, #2d3748 0%, #1a202c 100%); color: #d4af7a; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; color: #2d3748; }}
        .footer {{ text-align: center; color: #718096; font-size: 12px; margin-top: 30px; }}
        .success {{ background: #48bb78; color: white; padding: 10px; border-radius: 5px; text-align: center; margin-bottom: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Kitab</h1>
            <p>Paiement confirmé ✓</p>
        </div>
        <div class='success'>
            ✓ Votre paiement a été traité avec succès
        </div>
        <div class='content'>
            <h2>Merci pour votre achat !</h2>
            <p>Vous avez acheté : <strong>{videoTitle}</strong></p>
            <p>La vidéo est maintenant disponible dans votre bibliothèque.</p>
            <p style='text-align: center; margin: 30px 0;'>
                <a href='{{frontendUrl}}/my-videos' style='background: #d4af7a; color: #1a202c; padding: 15px 40px; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                    → Voir ma bibliothèque
                </a>
            </p>
        </div>
        <div class='footer'>
            <p>© 2025 Kitab. Tous droits réservés.</p>
            <p style='margin-top: 10px;'>Paiement sécurisé par Stripe</p>
        </div>
    </div>
</body>
</html>";

                await _mailService.SendEmailAsync(email, subject, body);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erreur envoi email: {ex.Message}");
        }
    }

    /// <summary>
    /// Génère un code d'achat unique
    /// </summary>
    private string GeneratePurchaseCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        string code;
        
        do
        {
            code = new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        while (_context.VideoPurchases.Any(vp => vp.PurchaseCode == code));
        
        return code;
    }
}

