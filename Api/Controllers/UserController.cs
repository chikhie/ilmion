using Microsoft.AspNetCore.Mvc;
using MuslimAds.Infra;
using MuslimAds.Api.Services;
using Microsoft.AspNetCore.Authorization;
using MuslimAds.Api.Dtos;
using MuslimAds.Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace MuslimAds.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IUserProvider _userProvider;

    public UserController(IUserProvider userProvider, UserManager<UserEntity> userManager)
    {
        _userProvider = userProvider;
        _userManager = userManager;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = _userProvider.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(userId);
        }

        return Ok(new
        {
            Email = user.Email,
            Company = user.Company
        });
    }
}