using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;
using Ilmanar.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModuleController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ModuleController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Récupère tous les modules d'une matière
    /// </summary>
    [HttpGet("subject/{subjectId}")]
    public async Task<ActionResult<List<ModuleResponseDto>>> GetModulesBySubject(int subjectId)
    {
        var modules = await _context.Modules
            .Include(m => m.Subject)
            .Include(m => m.Chapters)
            .Where(m => m.SubjectId == subjectId)
            .OrderBy(m => m.Label)
            .Select(m => new ModuleResponseDto
            {
                Id = m.Id,
                Label = m.Label,
                SubjectId = m.SubjectId,
                SubjectName = m.Subject.Label,
                ChapterCount = m.Chapters.Count,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            })
            .ToListAsync();

        return Ok(modules);
    }

    /// <summary>
    /// Récupère un module par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ModuleResponseDto>> GetModule(Guid id)
    {
        var module = await _context.Modules
            .Include(m => m.Subject)
            .Include(m => m.Chapters)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (module == null)
            return NotFound("Module non trouvé");

        var moduleDto = new ModuleResponseDto
        {
            Id = module.Id,
            Label = module.Label,
            SubjectId = module.SubjectId,
            SubjectName = module.Subject.Label,
            ChapterCount = module.Chapters.Count,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt
        };

        return Ok(moduleDto);
    }

    /// <summary>
    /// Crée un nouveau module
    /// </summary>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<ModuleResponseDto>> CreateModule(CreateModuleDto dto)
    {
        // Vérifier que la matière existe
        var subject = await _context.Subjects.FindAsync(dto.SubjectId);
        if (subject == null)
            return NotFound("Matière non trouvée");

        var module = new ModuleEntity
        {
            Id = Guid.NewGuid(),
            Label = dto.Label,
            SubjectId = dto.SubjectId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Modules.Add(module);
        await _context.SaveChangesAsync();

        var moduleDto = new ModuleResponseDto
        {
            Id = module.Id,
            Label = module.Label,
            SubjectId = module.SubjectId,
            SubjectName = subject.Label,
            ChapterCount = 0,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt
        };

        return CreatedAtAction(nameof(GetModule), new { id = module.Id }, moduleDto);
    }

    /// <summary>
    /// Met à jour un module
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<ModuleResponseDto>> UpdateModule(Guid id, UpdateModuleDto dto)
    {
        var module = await _context.Modules
            .Include(m => m.Subject)
            .Include(m => m.Chapters)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (module == null)
            return NotFound("Module non trouvé");

        if (dto.Label != null)
            module.Label = dto.Label;

        module.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var moduleDto = new ModuleResponseDto
        {
            Id = module.Id,
            Label = module.Label,
            SubjectId = module.SubjectId,
            SubjectName = module.Subject.Label,
            ChapterCount = module.Chapters.Count,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt
        };

        return Ok(moduleDto);
    }

    /// <summary>
    /// Supprime un module
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult> DeleteModule(Guid id)
    {
        var module = await _context.Modules.FindAsync(id);

        if (module == null)
            return NotFound("Module non trouvé");

        _context.Modules.Remove(module);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
