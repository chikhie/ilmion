using Microsoft.AspNetCore.Identity;

namespace Ilmanar.Infra.Entities;

public class UserEntity : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    // Informations de profil
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePicture { get; set; } // URL ou chemin de la photo
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Relations
    public ICollection<ModulePurchaseEntity> PurchasedModules { get; set; } = new List<ModulePurchaseEntity>();
}
