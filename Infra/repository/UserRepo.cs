using MuslimAds.Infra;
using Microsoft.AspNetCore.Identity;
using MuslimAds.Infra.Entities;

public class UserRepo
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserEntity> _userManager;

    public UserRepo(ApplicationDbContext context, UserManager<UserEntity> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<UserEntity?> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
}