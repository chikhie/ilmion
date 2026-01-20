using Ilmanar.Api.Dtos;
using Ilmanar.Infra.Services;
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly QuizService _quizService;

    public GamesController(ApplicationDbContext context, QuizService quizService)
    {
        _context = context;
        _quizService = quizService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GameDto>>> GetAllGames()
    {
        var games = await _context.Games
            .Select(g => new GameDto
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                Type = g.Type,
                ThumbnailPath = g.ThumbnailPath
            })
            .ToListAsync();

        return Ok(games);
    }

    [HttpGet("quizzes")]
    public async Task<ActionResult<List<GameDto>>> GetQuizzes([FromQuery] int? count)
    {
        var quizzes = await _context.Games
            .Where(g => g.Type == GameType.Quiz)
            .Select(g => new GameDto
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                Type = g.Type,
                ThumbnailPath = g.ThumbnailPath
            })
            .ToListAsync();

        foreach (var quiz in quizzes)
        {
            quiz.Questions = await _quizService.GetQuestionsForGameAsync(quiz.Id, count);
        }

        return Ok(quizzes);
    }

}
