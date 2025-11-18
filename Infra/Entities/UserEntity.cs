using Microsoft.AspNetCore.Identity;

namespace KitabStock.Infra.Entities;

public class UserEntity : IdentityUser
{
    // Collection des vidéos achetées par l'utilisateur
    public ICollection<VideoPurchaseEntity> PurchasedVideos { get; set; } = new List<VideoPurchaseEntity>();
    
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}