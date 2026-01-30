using Ilmanar.Api.Models;
using Ilmanar.Api.Services;
using Ilmanar.Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressionController : ControllerBase
{
    private readonly IProgressionService _progressionService;
    private readonly IUserProvider _userProvider;

    public ProgressionController(IProgressionService progressionService, IUserProvider userProvider)
    {
        _progressionService = progressionService;
        _userProvider = userProvider;
    }

    [HttpGet]
    public async Task<ActionResult<ProgressionViewModel>> GetProgression()
    {
        var userId = _userProvider.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var progression = await _progressionService.GetUserProgressionAsync(userId);
        return Ok(progression);
    }
}
