using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ilmanar.Infra.Entities;
using Ilmanar.Api.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IWebHostEnvironment _environment;

    public UserController(
        UserManager<UserEntity> userManager,
        IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _environment = environment;
    }

    /// <summary>
    /// Récupère toutes les informations de l'utilisateur connecté
    /// </summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        // Récupérer tous les claims nameidentifier et prendre celui qui ressemble à un GUID (l'ID)
        var nameIdentifierClaims = User.FindAll(ClaimTypes.NameIdentifier).ToList();
        var userId = nameIdentifierClaims.FirstOrDefault(c => Guid.TryParse(c.Value, out _))?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Token invalide : ID utilisateur manquant" });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "Utilisateur non trouvé" });
        }

        var userData = new UserProfileDto
        {
            Id = user.Id,
            Username = user.UserName ?? "",
            Email = user.Email ?? "",
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = GetProfilePictureUrl(user.ProfilePicture),
            DateOfBirth = user.DateOfBirth
        };

        return Ok(userData);
    }

    /// <summary>
    /// Met à jour le profil de l'utilisateur
    /// </summary>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        // Récupérer l'ID utilisateur depuis les claims
        var nameIdentifierClaims = User.FindAll(ClaimTypes.NameIdentifier).ToList();
        var userId = nameIdentifierClaims.FirstOrDefault(c => Guid.TryParse(c.Value, out _))?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Token invalide : ID utilisateur manquant" });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "Utilisateur non trouvé" });
        }

        // Mettre à jour les champs
        if (!string.IsNullOrWhiteSpace(dto.FirstName))
            user.FirstName = dto.FirstName;
        
        if (!string.IsNullOrWhiteSpace(dto.LastName))
            user.LastName = dto.LastName;
        
        if (!string.IsNullOrWhiteSpace(dto.Username) && dto.Username != user.UserName)
        {
            // Vérifier si le nom d'utilisateur est déjà pris
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                return BadRequest(new { message = "Ce nom d'utilisateur est déjà pris" });
            }
            user.UserName = dto.Username;
        }
        
        if (dto.DateOfBirth.HasValue)
            user.DateOfBirth = dto.DateOfBirth;

        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = "Erreur lors de la mise à jour du profil", errors = result.Errors });
        }

        return Ok(new { message = "Profil mis à jour avec succès" });
    }

    /// <summary>
    /// Upload de la photo de profil
    /// </summary>
    [HttpPost("profile/picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "Aucun fichier fourni" });
        }

        // Vérifier le type de fichier
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { message = "Format de fichier non supporté. Utilisez JPG, PNG ou GIF" });
        }

        // Vérifier la taille (max 5MB)
        if (file.Length > 5 * 1024 * 1024)
        {
            return BadRequest(new { message = "Le fichier est trop volumineux (max 5MB)" });
        }

        // Récupérer l'ID utilisateur depuis les claims
        var nameIdentifierClaims = User.FindAll(ClaimTypes.NameIdentifier).ToList();
        var userId = nameIdentifierClaims.FirstOrDefault(c => Guid.TryParse(c.Value, out _))?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Token invalide : ID utilisateur manquant" });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "Utilisateur non trouvé" });
        }

        try
        {
            // Créer le dossier de profils s'il n'existe pas
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Supprimer l'ancienne photo si elle existe (ProfilePicture contient uniquement le nom du fichier)
            if (!string.IsNullOrEmpty(user.ProfilePicture))
            {
                var oldPhotoPath = Path.Combine(_environment.WebRootPath, "uploads", "profiles", user.ProfilePicture);
                if (System.IO.File.Exists(oldPhotoPath))
                {
                    System.IO.File.Delete(oldPhotoPath);
                }
            }

            // Générer un nom de fichier unique
            var fileName = $"{user.Id}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Sauvegarder le fichier
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Stocker uniquement le nom du fichier
            user.ProfilePicture = fileName;
            user.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return Ok(new { 
                message = "Photo de profil mise à jour avec succès",
                profilePicture = GetProfilePictureUrl(fileName)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erreur lors de l'upload: {ex.Message}" });
        }
    }

    /// <summary>
    /// Supprimer la photo de profil
    /// </summary>
    [HttpDelete("profile/picture")]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        // Récupérer l'ID utilisateur depuis les claims
        var nameIdentifierClaims = User.FindAll(ClaimTypes.NameIdentifier).ToList();
        var userId = nameIdentifierClaims.FirstOrDefault(c => Guid.TryParse(c.Value, out _))?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Token invalide : ID utilisateur manquant" });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "Utilisateur non trouvé" });
        }

        if (!string.IsNullOrEmpty(user.ProfilePicture))
        {
            // Construire le chemin complet du fichier (ProfilePicture contient uniquement le nom du fichier)
            var photoPath = Path.Combine(_environment.WebRootPath, "uploads", "profiles", user.ProfilePicture);
            if (System.IO.File.Exists(photoPath))
            {
                System.IO.File.Delete(photoPath);
            }

            user.ProfilePicture = null;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        return Ok(new { message = "Photo de profil supprimée avec succès" });
    }

    /// <summary>
    /// Changer le mot de passe
    /// </summary>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        // Récupérer l'ID utilisateur depuis les claims
        var nameIdentifierClaims = User.FindAll(ClaimTypes.NameIdentifier).ToList();
        var userId = nameIdentifierClaims.FirstOrDefault(c => Guid.TryParse(c.Value, out _))?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Token invalide : ID utilisateur manquant" });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "Utilisateur non trouvé" });
        }

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        
        if (!result.Succeeded)
        {
            return BadRequest(new { 
                message = "Erreur lors du changement de mot de passe",
                errors = result.Errors 
            });
        }

        return Ok(new { message = "Mot de passe modifié avec succès" });
    }

    /// <summary>
    /// Méthode helper pour construire l'URL complète de la photo de profil
    /// </summary>
    private string? GetProfilePictureUrl(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return null;
        
        return $"{Request.Scheme}://{Request.Host}/uploads/profiles/{fileName}";
    }
}
