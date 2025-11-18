using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using KitabStock.Infra;
using KitabStock.Infra.Entities;
using KitabStock.Api.Dtos;
using KitabStock.Api.Interfaces;

namespace KitabStock.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestPurchaseController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IMailService _mailService;

    public GuestPurchaseController(
        ApplicationDbContext context,
        UserManager<UserEntity> userManager,
        IMailService mailService)
    {
        _context = context;
        _userManager = userManager;
        _mailService = mailService;
    }

    /// <summary>
    /// Acheter une vidéo sans compte utilisateur (génère un code d'achat et crée un user temporaire)
    /// </summary>
    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseVideoAsGuest([FromBody] GuestPurchaseVideoRequest request)
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

        // Créer ou récupérer un user avec cet email
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Créer un user temporaire avec juste l'email
            user = new UserEntity
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = false // Pas besoin de confirmer pour un achat invité
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(500, "Erreur lors de la création du compte temporaire.");
            }
        }

        // Générer un code d'achat unique
        var purchaseCode = GeneratePurchaseCode();

        // Créer l'achat
        var purchase = new VideoPurchaseEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            VideoId = request.VideoId,
            PurchaseCode = purchaseCode,
            PricePaid = video.Price,
            PurchaseDate = DateTime.UtcNow,
            Status = PurchaseStatus.Pending, // En attente de confirmation Stripe
            StripePaymentIntentId = request.StripePaymentIntentId
        };

        _context.VideoPurchases.Add(purchase);
        await _context.SaveChangesAsync();

        // Envoyer l'email avec le code d'achat
        await SendPurchaseCodeEmailAsync(request.Email, video.Title, purchaseCode);

        return Ok(new VideoPurchaseResponse
        {
            Id = purchase.Id,
            VideoId = video.Id,
            VideoTitle = video.Title,
            UserId = user.Id,
            PurchaseCode = purchaseCode,
            PurchaseDate = purchase.PurchaseDate,
            PricePaid = purchase.PricePaid,
            Status = purchase.Status.ToString()
        });
    }

    /// <summary>
    /// Accéder à une vidéo avec un code d'achat
    /// </summary>
    [HttpPost("download")]
    public async Task<IActionResult> DownloadVideo([FromBody] DownloadVideoRequest request)
    {
        var purchase = await _context.VideoPurchases.FirstOrDefaultAsync(vp => vp.PurchaseCode == request.PurchaseCode);
        if (purchase == null)
        {
            return NotFound("Code d'achat non trouvé.");
        }

        if (purchase.Status != PurchaseStatus.Completed)
        {
            return BadRequest("Achat non valide.");
        }
        var video = await _context.Videos.FindAsync(purchase.VideoId);
        if (video == null)
        {
            return NotFound("Vidéo non trouvée.");
        }
        var zipPath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "videos", purchase.VideoId.ToString(), video.Title + ".zip");
        if (!System.IO.File.Exists(zipPath))
        {
            return NotFound(new { message = "Fichier ZIP non trouvé" });
        }

        return PhysicalFile(zipPath, "application/zip", video.FilenameDownload);
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

    /// <summary>
    /// Envoie l'email avec le code d'achat
    /// </summary>
    private async Task SendPurchaseCodeEmailAsync(string email, string videoTitle, string purchaseCode)
    {
        var subject = $"Votre code d'achat pour '{videoTitle}'";
        var body = $@"
            <h2>Merci pour votre achat !</h2>
            <p>Vous avez acheté la vidéo : <strong>{videoTitle}</strong></p>
            <p>Votre code d'achat est : <strong style='font-size: 24px; color: #007bff;'>{purchaseCode}</strong></p>
            <p>Conservez ce code précieusement. Il vous permettra d'accéder à votre vidéo à tout moment.</p>
            <p>Pour télécharger votre vidéo, utilisez ce code sur notre site.</p>
        ";

        await _mailService.SendEmailAsync(email, subject, body);
    }
}