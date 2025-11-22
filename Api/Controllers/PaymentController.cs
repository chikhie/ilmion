using Microsoft.AspNetCore.Authorization;
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
    private readonly IModulePurchaseRepo _purchaseRepo;
    private readonly ApplicationDbContext _context;
    private readonly IUserProvider _userProvider;

    public PaymentController(
        IStripePaymentService stripeService,
        IModulePurchaseRepo purchaseRepo,
        ApplicationDbContext context,
        IUserProvider userProvider)
    {
        _stripeService = stripeService;
        _purchaseRepo = purchaseRepo;
        _context = context;
        _userProvider = userProvider;
    }

    /// <summary>
    /// Créer une session de paiement Stripe
    /// </summary>
    [HttpPost("create-checkout-session")]
    [Authorize]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] CreatePaymentDto dto)
    {
        var userId = _userProvider.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // Vérifier si le module existe
        var module = await _context.Modules.FindAsync(dto.ModuleId);
        if (module == null)
        {
            return NotFound("Module non trouvé");
        }

        // Vérifier si le module est gratuit
        if (module.IsFree || module.Price <= 0)
        {
            return BadRequest("Ce module est gratuit, aucun paiement nécessaire");
        }

        // Vérifier si l'utilisateur a déjà acheté ce module
        var alreadyPurchased = await _purchaseRepo.HasUserPurchasedModuleAsync(userId, dto.ModuleId);
        if (alreadyPurchased)
        {
            return BadRequest("Vous avez déjà acheté ce module");
        }

        try
        {
            var user = await _context.Users.FindAsync(userId);
            
            // Créer la session Stripe
            var session = await _stripeService.CreateCheckoutSessionAsync(
                module, 
                userId, 
                user?.Email
            );

            // Créer l'enregistrement d'achat en attente
            var purchase = new ModulePurchaseEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ModuleId = module.Id,
                AmountPaid = module.Price,
                Currency = "EUR",
                StripeSessionId = session.Id,
                Status = PurchaseStatus.Pending,
                PurchaseDate = DateTime.UtcNow
            };

            await _purchaseRepo.CreateAsync(purchase);

            return Ok(new PaymentSessionDto
            {
                SessionId = session.Id,
                SessionUrl = session.Url,
                PurchaseId = purchase.Id
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
    [Authorize]
    public async Task<IActionResult> GetPaymentStatus(string sessionId)
    {
        var purchase = await _purchaseRepo.GetByStripeSessionIdAsync(sessionId);
        
        if (purchase == null)
        {
            return NotFound("Paiement non trouvé");
        }

        var userId = _userProvider.GetUserId();
        if (purchase.UserId != userId)
        {
            return Forbid();
        }

        return Ok(new
        {
            status = purchase.Status.ToString(),
            purchaseId = purchase.Id,
            moduleId = purchase.ModuleId,
            amount = purchase.AmountPaid,
            purchaseDate = purchase.PurchaseDate,
            completedDate = purchase.CompletedDate
        });
    }

    /// <summary>
    /// Récupérer l'historique des achats de l'utilisateur
    /// </summary>
    [HttpGet("my-purchases")]
    [Authorize]
    public async Task<IActionResult> GetMyPurchases()
    {
        var userId = _userProvider.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var purchases = await _purchaseRepo.GetUserPurchasesAsync(userId);
        
        var purchaseDtos = purchases.Select(p => new ModulePurchaseDto
        {
            Id = p.Id,
            ModuleId = p.ModuleId,
            ModuleTitle = p.Module.Title,
            SubjectName = p.Module.Subject?.Label ?? "",
            AmountPaid = p.AmountPaid,
            Currency = p.Currency,
            Status = p.Status.ToString(),
            PurchaseDate = p.PurchaseDate,
            CompletedDate = p.CompletedDate
        }).ToList();

        return Ok(purchaseDtos);
    }

    /// <summary>
    /// Vérifier si l'utilisateur a acheté un module spécifique
    /// </summary>
    [HttpGet("has-access/{moduleId}")]
    [Authorize]
    public async Task<IActionResult> HasAccess(Guid moduleId)
    {
        var userId = _userProvider.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // Vérifier si le module existe et est gratuit
        var module = await _context.Modules.FindAsync(moduleId);
        if (module == null)
        {
            return NotFound("Module non trouvé");
        }

        if (module.IsFree)
        {
            return Ok(new { hasAccess = true, reason = "free" });
        }

        // Vérifier si l'utilisateur a acheté le module
        var hasPurchased = await _purchaseRepo.HasUserPurchasedModuleAsync(userId, moduleId);
        
        return Ok(new { hasAccess = hasPurchased, reason = hasPurchased ? "purchased" : "not_purchased" });
    }

    /// <summary>
    /// Page de succès après paiement (pour redirection)
    /// </summary>
    [HttpGet("success")]
    public IActionResult PaymentSuccess([FromQuery] string session_id)
    {
        // Cette route peut rediriger vers le frontend ou afficher une page de confirmation
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

