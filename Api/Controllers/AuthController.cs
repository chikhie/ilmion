using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MuslimAds.Infra.Entities;
using MuslimAds.Api.Interfaces; 
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace MuslimAds.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly IMailService _mailService; // Ajoutez cette ligne

    // Modifiez le constructeur pour inclure IMailService
    public AuthController(
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager,
        IMailService mailService) // Ajoutez mailService ici
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mailService = mailService; // Initialisez _mailService
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("Utilisateur non trouvé.");
        }

        // Ajoutez cette vérification pour l'e-mail confirmé
        if (!user.EmailConfirmed)
        {
            return Unauthorized("Votre e-mail n'est pas confirmé. Veuillez vérifier votre boîte de réception.");
        }

        var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return Ok("Connexion réussie.");
        }
        return Unauthorized("Tentative de connexion invalide.");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string email, string password, string company)
    {
        var user = new UserEntity { UserName = email, Email = email, Company = company };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            // Générer le jeton de confirmation d'e-mail
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); // Encoder le jeton pour l'URL

            // Créer le lien de confirmation
            var confirmationLink = Url.Action(
                "ConfirmEmail", // Nom de l'action pour la confirmation d'e-mail
                "Auth",         // Nom du contrôleur (AuthController)
                new { userId = user.Id, token = token },
                Request.Scheme); // Utilisez le schéma de la requête (http/https)

            // Envoyer l'e-mail de confirmation
            await _mailService.SendEmailAsync(
                email,
                "Confirmez votre e-mail pour MuslimAds",
                $"Veuillez confirmer votre compte en cliquant sur ce lien : <a href='{HtmlEncoder.Default.Encode(confirmationLink!)}'>lien</a>");

            return Ok("Utilisateur créé avec succès. Veuillez vérifier votre e-mail pour confirmer votre compte.");
        }

        return BadRequest(result.Errors);
    }

    // Nouvel endpoint pour la confirmation d'e-mail
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return BadRequest("Lien de confirmation invalide.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Impossible de charger l'utilisateur avec l'ID '{userId}'.");
        }

        token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)); // Décoder le jeton
        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
            return Ok("Votre e-mail a été confirmé avec succès.");
        }

        return BadRequest("Erreur lors de la confirmation de votre e-mail.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Utilisateur déconnecté avec succès.");
    }
}