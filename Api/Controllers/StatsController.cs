using Ilmanar.Api.Dtos;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]" )]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class StatsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StatsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var totalScore = await _context.UserGameScores
            .Where(s => s.UserId == userId)
            .SumAsync(s => s.Score);

        var globalProgress = 0; // Placeholder for now

        return new DashboardStatsDto
        {
            GlobalScore = totalScore,
            GlobalProgress = globalProgress,
            LearningTimeSeconds = 0,
            FormattedLearningTime = "0h00"
        };
    }
}
