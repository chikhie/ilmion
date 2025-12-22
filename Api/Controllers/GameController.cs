using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GameController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetGames()
    {
        return await _context.Games
            .Select(g => new { g.Id, g.Title, g.Type, g.Difficulty, g.ThumbnailPath, g.IsPremium })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameEntity>> GetGame(Guid id)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        return game;
    }
}
