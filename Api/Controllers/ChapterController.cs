using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;
using Ilmanar.Infra.repository;
using Ilmanar.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChapterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ISubscriptionRepo _subscriptionRepo;

    public ChapterController(ApplicationDbContext context, ISubscriptionRepo subscriptionRepo)
    {
        _context = context;
        _subscriptionRepo = subscriptionRepo;
    }

    /// <summary>
    /// Récupère tous les chapitres d'un module
    /// </summary>
    [HttpGet("module/{moduleId}")]
    public async Task<ActionResult<List<ChapterResponseDto>>> GetChaptersByModule(Guid moduleId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var hasSubscription = !string.IsNullOrEmpty(userId) && await _subscriptionRepo.HasActiveSubscriptionAsync(userId);

        var chapters = await _context.Chapters
            .Include(c => c.Module)
                .ThenInclude(m => m.Subject)
            .Where(c => c.ModuleId == moduleId)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        var chapterDtos = chapters.Select(c => new ChapterResponseDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            DisplayOrder = c.DisplayOrder,
            DurationMinutes = c.DurationMinutes,
            // Accès : abonnement actif OU premier chapitre du module (DisplayOrder = 1)
            HasAccess = hasSubscription || c.DisplayOrder == 1,
            IsFree = c.DisplayOrder == 1, // Le premier chapitre est gratuit
            ModuleId = c.ModuleId,
            ModuleName = c.Module.Label,
            SubjectId = c.Module.SubjectId,
            SubjectName = c.Module.Subject.Label,
            VideoId = c.VideoId,
            VideoUrl = c.VideoUrl,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return Ok(chapterDtos);
    }

    /// <summary>
    /// Récupère un chapitre par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ChapterResponseDto>> GetChapter(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var hasSubscription = !string.IsNullOrEmpty(userId) && await _subscriptionRepo.HasActiveSubscriptionAsync(userId);

        var chapter = await _context.Chapters
            .Include(c => c.Module)
                .ThenInclude(m => m.Subject)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (chapter == null)
            return NotFound("Chapitre non trouvé");

        var chapterDto = new ChapterResponseDto
        {
            Id = chapter.Id,
            Title = chapter.Title,
            Description = chapter.Description,
            DisplayOrder = chapter.DisplayOrder,
            DurationMinutes = chapter.DurationMinutes,
            // Accès : abonnement actif OU premier chapitre du module (DisplayOrder = 1)
            HasAccess = hasSubscription || chapter.DisplayOrder == 1,
            IsFree = chapter.DisplayOrder == 1, // Le premier chapitre est gratuit
            ModuleId = chapter.ModuleId,
            ModuleName = chapter.Module.Label,
            SubjectId = chapter.Module.SubjectId,
            SubjectName = chapter.Module.Subject.Label,
            VideoId = chapter.VideoId,
            VideoUrl = chapter.VideoUrl,
            Content = chapter.Content,
            CreatedAt = chapter.CreatedAt,
            UpdatedAt = chapter.UpdatedAt
        };

        return Ok(chapterDto);
    }

    /// <summary>
    /// Crée un nouveau chapitre
    /// </summary>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<ChapterResponseDto>> CreateChapter(CreateChapterDto dto)
    {
        // Vérifier que le module existe
        var module = await _context.Modules.FindAsync(dto.ModuleId);
        if (module == null)
            return NotFound("Module non trouvé");

        var chapter = new ChapterEntity
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            DisplayOrder = dto.DisplayOrder,
            DurationMinutes = dto.DurationMinutes,
            ModuleId = dto.ModuleId,
            VideoId = dto.VideoId,
            VideoUrl = dto.VideoUrl,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Chapters.Add(chapter);
        await _context.SaveChangesAsync();

        var chapterDto = new ChapterResponseDto
        {
            Id = chapter.Id,
            Title = chapter.Title,
            Description = chapter.Description,
            DisplayOrder = chapter.DisplayOrder,
            DurationMinutes = chapter.DurationMinutes,
            HasAccess = true, // L'admin a toujours accès
            IsFree = chapter.DisplayOrder == 1,
            ModuleId = chapter.ModuleId,
            ModuleName = module.Label,
            VideoId = chapter.VideoId,
            VideoUrl = chapter.VideoUrl,
            Content = chapter.Content,
            CreatedAt = chapter.CreatedAt,
            UpdatedAt = chapter.UpdatedAt
        };

        return CreatedAtAction(nameof(GetChapter), new { id = chapter.Id }, chapterDto);
    }

    /// <summary>
    /// Met à jour un chapitre
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<ChapterResponseDto>> UpdateChapter(Guid id, UpdateChapterDto dto)
    {
        var chapter = await _context.Chapters
            .Include(c => c.Module)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (chapter == null)
            return NotFound("Chapitre non trouvé");

        if (dto.Title != null)
            chapter.Title = dto.Title;
        if (dto.Description != null)
            chapter.Description = dto.Description;
        if (dto.DisplayOrder != null)
            chapter.DisplayOrder = dto.DisplayOrder.Value;
        if (dto.DurationMinutes != null)
            chapter.DurationMinutes = dto.DurationMinutes.Value;
        if (dto.VideoId != null)
            chapter.VideoId = dto.VideoId;
        if (dto.VideoUrl != null)
            chapter.VideoUrl = dto.VideoUrl;
        if (dto.Content != null)
            chapter.Content = dto.Content;

        chapter.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var chapterDto = new ChapterResponseDto
        {
            Id = chapter.Id,
            Title = chapter.Title,
            Description = chapter.Description,
            DisplayOrder = chapter.DisplayOrder,
            DurationMinutes = chapter.DurationMinutes,
            HasAccess = true, // L'admin a toujours accès
            IsFree = chapter.DisplayOrder == 1,
            ModuleId = chapter.ModuleId,
            ModuleName = chapter.Module.Label,
            VideoId = chapter.VideoId,
            VideoUrl = chapter.VideoUrl,
            Content = chapter.Content,
            CreatedAt = chapter.CreatedAt,
            UpdatedAt = chapter.UpdatedAt
        };

        return Ok(chapterDto);
    }

    /// <summary>
    /// Supprime un chapitre
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult> DeleteChapter(Guid id)
    {
        var chapter = await _context.Chapters.FindAsync(id);

        if (chapter == null)
            return NotFound("Chapitre non trouvé");

        _context.Chapters.Remove(chapter);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Récupère le contenu d'un fichier de chapitre (JSON)
    /// </summary>
    [HttpGet("{chapterId}/file/{fileName}")]
    public async Task<IActionResult> GetChapterFile(Guid chapterId, string fileName)
    {
        var chapter = await _context.Chapters.FindAsync(chapterId);
        
        if (chapter == null)
            return NotFound("Chapitre non trouvé");

        // Pour l'instant, retourner le contenu directement s'il est stocké en JSON
        if (!string.IsNullOrEmpty(chapter.Content))
        {
            return Content(chapter.Content, "application/json");
        }

        return NotFound("Contenu non disponible");
    }
}

