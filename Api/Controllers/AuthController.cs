using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MuslimAds.Infra.Entities;
using MuslimAds.Api.Interfaces; 
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using MuslimAds.Api.Dtos;
using System.Security.Cryptography; // AJOUTER CETTE LIGNE


namespace MuslimAds.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly IMailService _mailService; 
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager,
        IMailService mailService,
        IConfiguration configuration) 
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mailService = mailService; 
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.EmailConfirmed)
            return Unauthorized("Utilisateur inexistant ou e-mail non confirmé.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Mot de passe incorrect.");

        // Générer les claims (infos stockées dans le token)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        // Clé secrète + signature
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Création du token
        var accessToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        // Générer le refresh token
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Expire dans 7 jours
        await _userManager.UpdateAsync(user);

        return Ok(new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(accessToken),
            Expiration = accessToken.ValidTo,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = user.RefreshTokenExpiryTime
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new UserEntity { UserName = request.Email, Email = request.Email, Company = request.Company };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); 

            var frontendUrl = _configuration["FrontendUrl"];
            var confirmationLink = $"{frontendUrl}/confirm-email?userId={user.Id}&token={token}";

            await _mailService.SendEmailAsync(
                request.Email,
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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) 
        {
            return NotFound("Utilisateur non trouvé."); // Ou OK pour ne pas divulguer l'existence d'un compte
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var frontendUrl = _configuration["FrontendUrl"];
        var resetLink = $"{frontendUrl}/reset-password?email={HtmlEncoder.Default.Encode(request.Email)}&token={token}";

        await _mailService.SendEmailAsync(
            request.Email,
            "Réinitialiser votre mot de passe pour MuslimAds",
            $"Veuillez réinitialiser votre mot de passe en cliquant sur ce lien : <a href='{HtmlEncoder.Default.Encode(resetLink!)}'>lien</a>");

        return Ok("Un lien de réinitialisation de mot de passe a été envoyé à votre adresse e-mail.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("Utilisateur non trouvé.");
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

        if (result.Succeeded)
        {
            return Ok("Votre mot de passe a été réinitialisé avec succès.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var refreshToken = request.RefreshToken;
        var user = _userManager.Users.SingleOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

        if (user == null)
        {
            return Unauthorized("Refresh token invalide ou expiré.");
        }

        // Générer un nouvel access token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var newAccessToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2), // Nouvelle expiration pour l'access token
            signingCredentials: creds);

        // Générer un nouveau refresh token
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Nouvelle expiration pour le refresh token
        await _userManager.UpdateAsync(user);

        return Ok(new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            Expiration = newAccessToken.ValidTo,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiration = user.RefreshTokenExpiryTime
        });
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}