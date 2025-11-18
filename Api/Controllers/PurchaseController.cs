using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitabStock.Infra;
using KitabStock.Infra.Entities;
using KitabStock.Api.Dtos;
using System.Security.Claims;

namespace KitabStock.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PurchaseController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Acheter une vidéo
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> PurchaseVideo([FromBody] PurchaseVideoRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
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

        // Générer un code d'achat unique
        var purchaseCode = GeneratePurchaseCode();

        // Créer l'achat
        var purchase = new VideoPurchaseEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            VideoId = request.VideoId,
            PurchaseCode = purchaseCode,
            PricePaid = video.Price,
            PurchaseDate = DateTime.UtcNow,
            Status = PurchaseStatus.Completed,
            StripePaymentIntentId = request.StripePaymentIntentId
        };

        _context.VideoPurchases.Add(purchase);
        await _context.SaveChangesAsync();

        return Ok(new VideoPurchaseResponse
        {
            Id = purchase.Id,
            VideoId = video.Id,
            VideoTitle = video.Title,
            UserId = userId,
            PurchaseDate = purchase.PurchaseDate,
            PricePaid = purchase.PricePaid,
            Status = purchase.Status.ToString()
        });
    }

    /// <summary>
    /// Obtenir toutes les vidéos achetées par l'utilisateur connecté
    /// </summary>
    [HttpGet("my-videos")]
    [Authorize]
    public async Task<IActionResult> GetMyPurchasedVideos()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Utilisateur non authentifié.");
        }

        var purchasedVideos = await _context.VideoPurchases
            .Where(vp => vp.UserId == userId && vp.Status == PurchaseStatus.Completed)
            .Include(vp => vp.Video)
            .Select(vp => new UserPurchasedVideosResponse
            {
                VideoId = vp.Video.Id,
                Title = vp.Video.Title,
                Description = vp.Video.Description,
                PricePaid = vp.PricePaid,
                PurchaseDate = vp.PurchaseDate
            })
            .OrderByDescending(v => v.PurchaseDate)
            .ToListAsync();

        return Ok(purchasedVideos);
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

