namespace Ilmanar.Api.Dtos;

/// <summary>
/// DTO pour afficher le profil utilisateur
/// </summary>
public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

/// <summary>
/// DTO pour mettre à jour le profil utilisateur
/// </summary>
public class UpdateProfileDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

/// <summary>
/// DTO pour changer le mot de passe
/// </summary>
public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

