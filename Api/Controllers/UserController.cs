using Microsoft.AspNetCore.Mvc;
using KitabStock.Infra;
using KitabStock.Api.Services;
using Microsoft.AspNetCore.Authorization;
using KitabStock.Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace KitabStock.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IUserProvider _userProvider;

    public UserController(
        UserManager<UserEntity> userManager, 
        IUserProvider userProvider)
    {
        _userManager = userManager;
        _userProvider = userProvider;
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
            UserName = user.UserName,
            Email = user.Email,
        });
    }
}