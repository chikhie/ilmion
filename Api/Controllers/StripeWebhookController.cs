using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Ilmanar.Infra.repository;
using Ilmanar.Infra.Entities;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeWebhookController : ControllerBase
{
    private readonly ISubscriptionRepo _subscriptionRepo;
    private readonly IConfiguration _configuration;
    private readonly ILogger<StripeWebhookController> _logger;

    public StripeWebhookController(
        ISubscriptionRepo subscriptionRepo,
        IConfiguration configuration,
        ILogger<StripeWebhookController> logger)
    {
        _subscriptionRepo = subscriptionRepo;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Webhook pour recevoir les événements Stripe (Payment Link)
    /// </summary>
    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebhook()
    {
        try
        {
            var webhookSecret = _configuration["Stripe:WebhookSecret"];
            var stripeSignature = Request.Headers["Stripe-Signature"].ToString();

            if (string.IsNullOrEmpty(stripeSignature))
            {
                _logger.LogError("❌ Header Stripe-Signature manquant");
                return BadRequest("Missing Stripe-Signature header");
            }

            // Lire le body RAW sans modification (méthode recommandée par Stripe)
            string json;
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                json = await reader.ReadToEndAsync();
            }

            _logger.LogInformation($"🔍 Webhook reçu - Body length: {json.Length}");
            _logger.LogInformation($"🔑 Secret: whsec_{webhookSecret?.Substring(6, Math.Min(10, (webhookSecret?.Length ?? 6) - 6))}...");
            _logger.LogInformation($"🔐 Signature: {stripeSignature.Substring(0, Math.Min(30, stripeSignature.Length))}...");

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                webhookSecret,
                throwOnApiVersionMismatch: false
            );

            _logger.LogInformation($"📥 Webhook Stripe reçu: {stripeEvent.Type}");

            // Gérer les différents types d'événements
            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    await HandleCheckoutSessionCompleted(stripeEvent);
                    break;
                    
                case "payment_intent.succeeded":
                    _logger.LogInformation("✅ Paiement réussi");
                    break;
                    
                case "payment_intent.payment_failed":
                    _logger.LogWarning("❌ Paiement échoué");
                    break;

                default:
                    _logger.LogInformation($"ℹ️ Événement non géré: {stripeEvent.Type}");
                    break;
            }

            return Ok();
        }
        catch (StripeException e)
        {
            _logger.LogError($"❌ Erreur Stripe Webhook: {e.Message}");
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Erreur générique: {ex.GetType().Name} - {ex.Message}");
            _logger.LogError($"StackTrace: {ex.StackTrace}");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gérer la complétion d'une session Checkout (paiement réussi)
    /// </summary>
    private async Task HandleCheckoutSessionCompleted(Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;
        if (session == null)
        {
            _logger.LogWarning("⚠️ Session Checkout est null");
            return;
        }

        _logger.LogInformation($"🎯 Session complétée: {session.Id}");

        // RÉCUPÉRER L'ID UTILISATEUR via Metadata
        string? userId = null;
        
        if (session.Metadata != null && session.Metadata.ContainsKey("user_id"))
        {
            userId = session.Metadata["user_id"];
            _logger.LogInformation($"👤 User ID trouvé dans Metadata: {userId}");
        }
        else if (!string.IsNullOrEmpty(session.ClientReferenceId))
        {
            userId = session.ClientReferenceId;
            _logger.LogInformation($"👤 User ID trouvé dans ClientReferenceId: {userId}");
        }

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError("❌ Impossible de trouver l'ID utilisateur dans la session Stripe");
            return;
        }

        // Vérifier si l'abonnement existe déjà
        var existingSubscription = await _subscriptionRepo.GetByStripeSessionIdAsync(session.Id);
        if (existingSubscription != null)
        {
            _logger.LogInformation($"ℹ️ Abonnement déjà créé pour cette session: {session.Id}");
            return;
        }

        // Créer l'abonnement dans la base de données
        var subscription = new SubscriptionEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = SubscriptionType.Annual,
            AmountPaid = (decimal)(session.AmountTotal ?? 1000) / 100, // Stripe utilise les centimes
            Currency = session.Currency?.ToUpper() ?? "EUR",
            StripeSessionId = session.Id,
            StripeCustomerId = session.CustomerId,
            Status = SubscriptionStatus.Active,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1), // Abonnement annuel
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _subscriptionRepo.CreateAsync(subscription);
        _logger.LogInformation($"✅ Abonnement créé avec succès pour l'utilisateur {userId}");
    }
}

