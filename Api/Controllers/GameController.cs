using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;

using System.Text.Json;
using System.Text.Json.Nodes;

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
        // Use AsNoTracking so we can modify ContentJson in memory without tracking/saving
        var game = await _context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            return NotFound();
        }

        // Return raw content without shuffling. 
        // Logic moved to client for Solo mode, or managed by MultiplayerService for consistency.
        // if (game.Type == GameType.Quiz && !string.IsNullOrEmpty(game.ContentJson)) { ... } // REMOVED SHUFFLE

        return game;
    }
}
