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

        // 1. Get followed modules
        // A module is followed if user has progress in any of its sections
        var userProgress = await _context.UserProgress
            .Include(up => up.Section)
            .ThenInclude(s => s.Chapter)
            .ThenInclude(c => c.Module)
            .ThenInclude(m => m.Subject)
            .Where(up => up.UserId == userId)
            .ToListAsync();

        var moduleIds = userProgress.Select(up => up.Section.Chapter.ModuleId).Distinct().ToList();

        var followedModules = new List<FollowedModuleDto>();

        foreach (var moduleId in moduleIds)
        {
            var module = await _context.Modules
                .Include(m => m.Subject)
                .Include(m => m.Chapters)
                .ThenInclude(c => c.Sections)
                .FirstOrDefaultAsync(m => m.Id == moduleId);

            if (module == null) continue;

            var totalSections = module.Chapters.SelectMany(c => c.Sections).Count();
            var completedSections = userProgress.Count(up => up.Section.Chapter.ModuleId == moduleId && up.IsCompleted);
            
            var progress = totalSections > 0 ? (int)((double)completedSections / totalSections * 100) : 0;

            followedModules.Add(new FollowedModuleDto
            {
                Id = module.Id,
                Label = module.Label,
                SubjectName = module.Subject.Label,
                ChapterCount = module.Chapters.Count,
                Progress = progress,
            });
        }

        // 2. Global Progress
        var globalProgress = followedModules.Any() ? (int)followedModules.Average(m => m.Progress) : 0;

        // 3. Learning Time (This Week)
        // Calculer le début de la semaine (Lundi)
        var today = DateTime.UtcNow.Date;
        var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        var startOfWeek = today.AddDays(-1 * diff);

        var learningTimeSeconds = await _context.UserLearningLogs
            .Where(l => l.UserId == userId && l.Date >= startOfWeek)
            .SumAsync(l => l.DurationSeconds);
            
        var hours = learningTimeSeconds / 3600;
        var minutes = (learningTimeSeconds % 3600) / 60;
        
        string formattedTime;
        if (hours > 0)
        {
            formattedTime = $"{hours}h{minutes:D2}";
        }
        else
        {
            formattedTime = $"{minutes}m";
        }

        return new DashboardStatsDto
        {
            FollowedModules = followedModules,
            GlobalProgress = globalProgress,
            LearningTimeSeconds = learningTimeSeconds,
            FormattedLearningTime = formattedTime
        };
    }

    [HttpPost("progress")]
    public async Task<IActionResult> UpdateProgress([FromBody] UpdateProgressDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var progress = await _context.UserProgress
            .FirstOrDefaultAsync(up => up.UserId == userId && up.SectionId == dto.SectionId);

        if (progress == null)
        {
            progress = new UserProgressEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SectionId = dto.SectionId,
                IsCompleted = dto.IsCompleted,
                CompletedAt = dto.IsCompleted ? DateTime.UtcNow : null
            };
            _context.UserProgress.Add(progress);
        }
        else
        {
            progress.IsCompleted = dto.IsCompleted;
            progress.CompletedAt = dto.IsCompleted ? DateTime.UtcNow : null;
            progress.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("time")]
    public async Task<IActionResult> LogTime([FromBody] LogTimeDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        var today = DateTime.UtcNow.Date;
        
        var log = await _context.UserLearningLogs
            .FirstOrDefaultAsync(l => l.UserId == userId && l.Date == today && l.ModuleId == dto.ModuleId);
            
        if (log == null)
        {
            log = new UserLearningLogEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = today,
                ModuleId = dto.ModuleId,
                DurationSeconds = dto.DurationSeconds
            };
            _context.UserLearningLogs.Add(log);
        }
        else
        {
            log.DurationSeconds += dto.DurationSeconds;
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}

