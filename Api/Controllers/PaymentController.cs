using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Ilmanar.Infra.Entities;
using Ilmanar.Infra.Services;
using Ilmanar.Infra.repository;
using Ilmanar.Infra;
using Ilmanar.Api.Dtos;
using Ilmanar.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IStripePaymentService _stripeService;
    private readonly ISubscriptionRepo _subscriptionRepo;
    private readonly ApplicationDbContext _context;
    private readonly IUserProvider _userProvider;
    private readonly IConfiguration _configuration;

    public PaymentController(
        IStripePaymentService stripeService,
        ISubscriptionRepo subscriptionRepo,
        ApplicationDbContext context,
        IUserProvider userProvider,
        IConfiguration configuration)
    {
        _stripeService = stripeService;
        _subscriptionRepo = subscriptionRepo;
        _context = context;
        _userProvider = userProvider;
        _configuration = configuration;
    }

    /// <summary>
    /// Créer une session de paiement Stripe pour abonnement annuel (10€/an)
    /// </summary>
    [HttpPost("create-subscription")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateSubscription()
    {
        var userId = _userProvider.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // Vérifier si l'utilisateur a déjà un abonnement actif
        var hasActiveSubscription = await _subscriptionRepo.HasActiveSubscriptionAsync(userId);
        if (hasActiveSubscription)
        {
            return BadRequest("Vous avez déjà un abonnement actif");
        }

        // Prix unique : 10€/an
        decimal price = 10.00m;
        string priceDescription = "Abonnement Premium Annuel";

        try
        {
            var user = await _context.Users.FindAsync(userId);
            
            // Créer la session Stripe
            var session = await _stripeService.CreateSubscriptionCheckoutSessionAsync(
                price,
                priceDescription,
                userId, 
                user?.Email
            );

            // Créer l'enregistrement d'abonnement en attente
            var subscription = new SubscriptionEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = SubscriptionType.Annual,
                AmountPaid = price,
                Currency = "EUR",
                StripeSessionId = session.Id,
                Status = SubscriptionStatus.Pending,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddYears(1),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _subscriptionRepo.CreateAsync(subscription);

            return Ok(new PaymentSessionDto
            {
                SessionId = session.Id,
                SessionUrl = session.Url,
                SubscriptionId = subscription.Id
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erreur lors de la création de la session de paiement: {ex.Message}" });
        }
    }

    /// <summary>
    /// Vérifier le statut d'un paiement
    /// </summary>
    [HttpGet("status/{sessionId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetPaymentStatus(string sessionId)
    {
        var subscription = await _subscriptionRepo.GetByStripeSessionIdAsync(sessionId);
        
        if (subscription == null)
        {
            return NotFound("Paiement non trouvé");
        }

        var userId = _userProvider.GetUserId();
        if (subscription.UserId != userId)
        {
            return Forbid();
        }

        return Ok(new
        {
            status = subscription.Status.ToString(),
            subscriptionId = subscription.Id,
            type = subscription.Type.ToString(),
            amount = subscription.AmountPaid,
            startDate = subscription.StartDate,
            endDate = subscription.EndDate
        });
    }

    /// <summary>
    /// Récupérer l'abonnement actif de l'utilisateur
    /// </summary>
    [HttpGet("my-subscription")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetMySubscription()
    {
        var userMail = _userProvider.GetUserId();
        if (string.IsNullOrEmpty(userMail))
        {
            return Unauthorized();
        }
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userMail);
        if (user == null)
        {
            return NotFound("Utilisateur non trouvé");
        }
        var userId = user.Id;

        var subscription = await _subscriptionRepo.GetActiveSubscriptionAsync(userId);
        
        if (subscription == null)
        {
            return Ok(new { hasSubscription = false });
        }

        var now = DateTime.UtcNow;
        var daysRemaining = (subscription.EndDate - now).Days;

        var subscriptionDto = new SubscriptionDto
        {
            Id = subscription.Id,
            Type = subscription.Type.ToString(),
            Status = subscription.Status.ToString(),
            AmountPaid = subscription.AmountPaid,
            Currency = subscription.Currency,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            CancelledDate = subscription.CancelledDate,
            IsActive = subscription.Status == SubscriptionStatus.Active && subscription.EndDate > now,
            DaysRemaining = daysRemaining > 0 ? daysRemaining : 0
        };

        return Ok(new { hasSubscription = true, subscription = subscriptionDto });
    }

    /// <summary>
    /// Vérifier si l'utilisateur a un abonnement actif
    /// </summary>
    [HttpGet("has-access")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> HasAccess()
    {
        var userMail = _userProvider.GetUserId();
        if (string.IsNullOrEmpty(userMail))
        {
            return Unauthorized();
        }
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userMail);
        if (user == null)
        {
            return NotFound("Utilisateur non trouvé");
        }
        var userId = user.Id;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var hasActiveSubscription = await _subscriptionRepo.HasActiveSubscriptionAsync(userId);
        
        return Ok(new { hasAccess = hasActiveSubscription });
    }

    /// <summary>
    /// Annuler un abonnement
    /// </summary>
    [HttpPost("cancel-subscription")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CancelSubscription()
    {
        var userId = _userProvider.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var subscription = await _subscriptionRepo.GetActiveSubscriptionAsync(userId);
        if (subscription == null)
        {
            return NotFound("Aucun abonnement actif trouvé");
        }

        subscription.Status = SubscriptionStatus.Cancelled;
        subscription.CancelledDate = DateTime.UtcNow;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _subscriptionRepo.UpdateAsync(subscription);

        // TODO: Annuler aussi dans Stripe si nécessaire

        return Ok(new { message = "Abonnement annulé avec succès" });
    }

    /// <summary>
    /// Page de succès après paiement (pour redirection)
    /// </summary>
    [HttpGet("success")]
    public IActionResult PaymentSuccess([FromQuery] string session_id)
    {
        return Ok(new { message = "Paiement réussi!", sessionId = session_id });
    }

    /// <summary>
    /// Page d'annulation de paiement
    /// </summary>
    [HttpGet("cancel")]
    public IActionResult PaymentCancel()
    {
        return Ok(new { message = "Paiement annulé" });
    }
}
