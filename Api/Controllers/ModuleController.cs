using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;
using Ilmanar.Infra.repository;
using Ilmanar.Api.Dtos;
using Ilmanar.Api.Services;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModuleController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IModulePurchaseRepo _purchaseRepo;
    private readonly IUserProvider _userProvider;

    public ModuleController(
        ApplicationDbContext context,
        IModulePurchaseRepo purchaseRepo,
        IUserProvider userProvider)
    {
        _context = context;
        _purchaseRepo = purchaseRepo;
        _userProvider = userProvider;
    }

    /// <summary>
    /// Récupère tous les modules d'une matière
    /// </summary>
    [HttpGet("subject/{subjectId}")]
    public async Task<IActionResult> GetModulesBySubject(int subjectId)
    {
        var userId = _userProvider.GetUserId();
        
        var modules = await _context.Modules
            .Include(m => m.Subject)
            .Where(m => m.SubjectId == subjectId)
            .ToListAsync();

        var moduleDtos = new List<ModuleResponseDto>();
        
        foreach (var m in modules)
        {
            bool hasAccess = m.IsFree;
            
            // Vérifier si l'utilisateur a acheté le module
            if (!hasAccess && !string.IsNullOrEmpty(userId))
            {
                hasAccess = await _purchaseRepo.HasUserPurchasedModuleAsync(userId, m.Id);
            }
            
            moduleDtos.Add(new ModuleResponseDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                DurationMinutes = m.DurationMinutes,
                Price = m.Price,
                IsFree = m.IsFree,
                HasAccess = hasAccess,
                SubjectName = m.Subject.Label,
                VideoId = m.VideoId,
                Content = m.Content,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            });
        }

        return Ok(moduleDtos);
    }

    /// <summary>
    /// Récupère un module par son ID avec ses sections
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetModuleById(Guid id)
    {
        var module = await _context.Modules
            .Include(m => m.Subject)
            .Include(m => m.Video)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (module == null)
        {
            return NotFound(new { message = "Module non trouvé" });
        }

        return Ok(module);
    }

    /// <summary>
    /// Crée un nouveau module
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateModule([FromBody] CreateModuleDto dto)
    {
        // Vérifier que la matière existe
        var subjectExists = await _context.Subjects.AnyAsync(s => s.Id == dto.SubjectId);
        if (!subjectExists)
        {
            return BadRequest(new { message = "Matière non trouvée" });
        }

        // Vérifier que la vidéo existe (si fournie)
        if (dto.VideoId.HasValue)
        {
            var videoExists = await _context.Videos.AnyAsync(v => v.Id == dto.VideoId.Value);
            if (!videoExists)
            {
                return BadRequest(new { message = "Vidéo non trouvée" });
            }
        }

        var module = new ModuleEntity
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            DurationMinutes = dto.DurationMinutes,
            Price = dto.Price,
            IsFree = dto.IsFree,
            SubjectId = dto.SubjectId,
            VideoId = dto.VideoId,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Modules.Add(module);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetModuleById), new { id = module.Id }, module);
    }

    /// <summary>
    /// Met à jour un module
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModule(Guid id, [FromBody] UpdateModuleDto dto)
    {
        var module = await _context.Modules.FindAsync(id);
        if (module == null)
        {
            return NotFound(new { message = "Module non trouvé" });
        }

        if (dto.Title != null) module.Title = dto.Title;
        if (dto.Description != null) module.Description = dto.Description;
        if (dto.DisplayOrder.HasValue) module.DisplayOrder = dto.DisplayOrder.Value;
        if (dto.DurationMinutes.HasValue) module.DurationMinutes = dto.DurationMinutes.Value;
        if (dto.Price.HasValue) module.Price = dto.Price.Value;
        if (dto.IsFree.HasValue) module.IsFree = dto.IsFree.Value;
        if (dto.VideoId.HasValue) module.VideoId = dto.VideoId.Value;
        if (dto.Content != null) module.Content = dto.Content;
        
        module.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(module);
    }

    /// <summary>
    /// Supprime un module
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModule(Guid id)
    {
        var module = await _context.Modules.FindAsync(id);
        if (module == null)
        {
            return NotFound(new { message = "Module non trouvé" });
        }

        _context.Modules.Remove(module);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Module supprimé avec succès" });
    }
}

