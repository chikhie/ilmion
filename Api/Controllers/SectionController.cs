using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;
using Ilmanar.Api.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SectionController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Récupère toutes les sections d'un chapitre
    /// </summary>
    [HttpGet("chapter/{chapterId}")]
    public async Task<ActionResult<List<SectionResponseDto>>> GetSectionsByChapter(Guid chapterId)
    {
        var sections = await _context.Sections
            .Include(s => s.SectionType)
            .Where(s => s.ChapterId == chapterId)
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new SectionResponseDto
            {
                Id = s.Id,
                Title = s.Title,
                Content = s.Content,
                SectionTypeId = s.SectionTypeId,
                SectionTypeLabel = s.SectionType.Label,
                DisplayOrder = s.DisplayOrder,
                ChapterId = s.ChapterId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync();

        return Ok(sections);
    }

    /// <summary>
    /// Récupère une section par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SectionResponseDto>> GetSection(Guid id)
    {
        var section = await _context.Sections
            .Include(s => s.SectionType)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (section == null)
            return NotFound("Section non trouvée");

        var sectionDto = new SectionResponseDto
        {
            Id = section.Id,
            Title = section.Title,
            Content = section.Content,
            SectionTypeId = section.SectionTypeId,
            SectionTypeLabel = section.SectionType.Label,
            DisplayOrder = section.DisplayOrder,
            ChapterId = section.ChapterId,
            CreatedAt = section.CreatedAt,
            UpdatedAt = section.UpdatedAt
        };

        return Ok(sectionDto);
    }

    /// <summary>
    /// Crée une nouvelle section
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SectionResponseDto>> CreateSection(CreateSectionDto dto)
    {
        // Vérifier que le chapitre existe
        var chapter = await _context.Chapters.FindAsync(dto.ChapterId);
        if (chapter == null)
            return NotFound("Chapitre non trouvé");

        // Vérifier que le type de section existe
        var sectionType = await _context.SectionTypes.FindAsync(dto.SectionTypeId);
        if (sectionType == null)
            return BadRequest("Type de section invalide");

        var section = new SectionEntity
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            SectionTypeId = dto.SectionTypeId,
            DisplayOrder = dto.DisplayOrder,
            ChapterId = dto.ChapterId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Sections.Add(section);
        await _context.SaveChangesAsync();

        // Recharger avec le type
        await _context.Entry(section).Reference(s => s.SectionType).LoadAsync();

        var sectionDto = new SectionResponseDto
        {
            Id = section.Id,
            Title = section.Title,
            Content = section.Content,
            SectionTypeId = section.SectionTypeId,
            SectionTypeLabel = section.SectionType.Label,
            DisplayOrder = section.DisplayOrder,
            ChapterId = section.ChapterId,
            CreatedAt = section.CreatedAt,
            UpdatedAt = section.UpdatedAt
        };

        return CreatedAtAction(nameof(GetSection), new { id = section.Id }, sectionDto);
    }

    /// <summary>
    /// Met à jour une section
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SectionResponseDto>> UpdateSection(Guid id, UpdateSectionDto dto)
    {
        var section = await _context.Sections
            .Include(s => s.SectionType)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (section == null)
            return NotFound("Section non trouvée");

        if (dto.Title != null)
            section.Title = dto.Title;
        if (dto.Content != null)
            section.Content = dto.Content;
        if (dto.SectionTypeId != null)
        {
            // Vérifier que le type existe
            var sectionType = await _context.SectionTypes.FindAsync(dto.SectionTypeId.Value);
            if (sectionType == null)
                return BadRequest("Type de section invalide");
            section.SectionTypeId = dto.SectionTypeId.Value;
        }
        if (dto.DisplayOrder != null)
            section.DisplayOrder = dto.DisplayOrder.Value;

        section.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Recharger le type si changé
        await _context.Entry(section).Reference(s => s.SectionType).LoadAsync();

        var sectionDto = new SectionResponseDto
        {
            Id = section.Id,
            Title = section.Title,
            Content = section.Content,
            SectionTypeId = section.SectionTypeId,
            SectionTypeLabel = section.SectionType.Label,
            DisplayOrder = section.DisplayOrder,
            ChapterId = section.ChapterId,
            CreatedAt = section.CreatedAt,
            UpdatedAt = section.UpdatedAt
        };

        return Ok(sectionDto);
    }

    /// <summary>
    /// Supprime une section
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteSection(Guid id)
    {
        var section = await _context.Sections.FindAsync(id);

        if (section == null)
            return NotFound("Section non trouvée");

        _context.Sections.Remove(section);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

