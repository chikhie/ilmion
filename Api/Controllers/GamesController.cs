using Microsoft.AspNetCore.Mvc;
using Ilmanar.Infra.Services;
using Ilmanar.Infra.Entities.Mongo;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly QuizService _quizService;
    private readonly GameService _gameService;

    public GamesController(QuizService quizService, GameService gameService)
    {
        _quizService = quizService;
        _gameService = gameService;
    }

    [HttpGet("quizzes")]
    public async Task<ActionResult<IEnumerable<QuizItem>>> GetQuizzes([FromQuery] int? count, [FromQuery] string? lang = "fr")
    {
        var questions = await _quizService.GetQuestionsForQuiz(count, lang);
        return Ok(questions);
    }
}
