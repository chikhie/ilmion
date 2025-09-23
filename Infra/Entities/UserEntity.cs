using Microsoft.AspNetCore.Identity;

namespace MuslimAds.Infra.Entities;

public class UserEntity : IdentityUser
{

    public string Company { get; set; } = string.Empty;
}