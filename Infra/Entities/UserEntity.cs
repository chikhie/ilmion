using Microsoft.AspNetCore.Identity;

namespace MuslimAds.Infra.Entities;

public class UserEntity : IdentityUser
{
    public string Company { get; set; } = string.Empty;
    public ICollection<AdEntity> Ads { get; set; } = new List<AdEntity>();
    public ICollection<WebsiteEntity> Websites { get; set; } = new List<WebsiteEntity>();
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}