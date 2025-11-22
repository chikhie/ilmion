using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;
using Ilmanar.Api.Dtos;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SubjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Récupère toutes les matières
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllSubjects()
    {
        var subjects = await _context.Subjects
            .Select(s => new SubjectResponseDto
            {
                Id = s.Id,
                Label = s.Label
            })
            .ToListAsync();

        return Ok(subjects);
    }

    /// <summary>
    /// Récupère une matière par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubjectById(int id)
    {
        var subject = await _context.Subjects
            .Where(s => s.Id == id)
            .Select(s => new SubjectResponseDto
            {
                Id = s.Id,
                Label = s.Label
            })
            .FirstOrDefaultAsync();

        if (subject == null)
        {
            return NotFound(new { message = "Matière non trouvée" });
        }

        return Ok(subject);
    }
}

