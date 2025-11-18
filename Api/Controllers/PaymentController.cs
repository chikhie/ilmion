using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitabStock.Infra;
using KitabStock.Infra.Payment;
using System.Security.Claims;

namespace KitabStock.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly StripePaymentService _stripeService;
    private readonly IConfiguration _configuration;

    public PaymentController(
        ApplicationDbContext context,
        StripePaymentService stripeService,
        IConfiguration configuration)
    {
        _context = context;
        _stripeService = stripeService;
        _configuration = configuration;
    }

    /// <summary>
    /// Créer une session de paiement Stripe pour un utilisateur connecté
    /// </summary>
    [HttpPost("create-checkout-session")]
    [Authorize]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("Utilisateur non authentifié.");
        }

        // Vérifier si la vidéo existe
        var video = await _context.Videos.FindAsync(request.VideoId);
        if (video == null)
        {
            return NotFound("Vidéo non trouvée.");
        }

        // Vérifier si l'utilisateur a déjà acheté cette vidéo
        var existingPurchase = await _context.VideoPurchases
            .FirstOrDefaultAsync(vp => vp.UserId == userId && vp.VideoId == request.VideoId);

        if (existingPurchase != null)
        {
            return BadRequest("Vous avez déjà acheté cette vidéo.");
        }

        var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
        var successUrl = $"{frontendUrl}/payment/success?session_id={{CHECKOUT_SESSION_ID}}";
        var cancelUrl = $"{frontendUrl}/payment/cancel";

        try
        {
            var session = await _stripeService.CreateCheckoutSessionForUser(
                video,
                userId,
                userEmail,
                successUrl,
                cancelUrl
            );

            return Ok(new
            {
                sessionId = session.Id,
                url = session.Url,
                publicKey = _configuration["Stripe:PublishableKey"]
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors de la création de la session de paiement: {ex.Message}");
        }
    }

    /// <summary>
    /// Créer une session de paiement Stripe pour un invité (sans compte)
    /// </summary>
    [HttpPost("create-guest-checkout-session")]
    public async Task<IActionResult> CreateGuestCheckoutSession([FromBody] CreateGuestCheckoutRequest request)
    {
        // Validation de l'email
        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
        {
            return BadRequest("Email invalide.");
        }

        // Vérifier si la vidéo existe
        var video = await _context.Videos.FindAsync(request.VideoId);
        if (video == null)
        {
            return NotFound("Vidéo non trouvée.");
        }

        var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
        var successUrl = $"{frontendUrl}/payment/success?session_id={{CHECKOUT_SESSION_ID}}";
        var cancelUrl = $"{frontendUrl}/payment/cancel";

        try
        {
            var session = await _stripeService.CreateCheckoutSessionForGuest(
                video,
                request.Email,
                successUrl,
                cancelUrl
            );

            return Ok(new
            {
                sessionId = session.Id,
                url = session.Url,
                publicKey = _configuration["Stripe:PublishableKey"]
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors de la création de la session de paiement: {ex.Message}");
        }
    }

    /// <summary>
    /// Vérifier le statut d'une session de paiement
    /// </summary>
    [HttpGet("session-status/{sessionId}")]
    public async Task<IActionResult> GetSessionStatus(string sessionId)
    {
        try
        {
            var session = await _stripeService.GetSession(sessionId);

            return Ok(new
            {
                status = session.PaymentStatus,
                customerEmail = session.CustomerEmail,
                amountTotal = session.AmountTotal / 100.0m, // Convertir de centimes en euros
                currency = session.Currency
            });
        }
        catch (Exception ex)
        {
            return NotFound($"Session non trouvée: {ex.Message}");
        }
    }
}

public class CreateCheckoutRequest
{
    public Guid VideoId { get; set; }
}

public class CreateGuestCheckoutRequest
{
    public Guid VideoId { get; set; }
    public string Email { get; set; } = string.Empty;
}

