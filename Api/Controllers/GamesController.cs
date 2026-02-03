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
    public async Task<ActionResult<IEnumerable<QuizItem>>> GetQuizzes([FromQuery] int? count, [FromQuery] string? lang = "fr", [FromQuery] string? themeId = null, [FromQuery] string? partId = null)
    {
        var questions = await _quizService.GetQuestionsForQuiz(count, lang, themeId, partId);
        return Ok(questions);
    }

    [HttpGet("quizzes/test")]
    public async Task<IActionResult> GetQuizzes()
    {
        var testData = new QuizItem
        {
            Text = "Test Question",
            CorrectAnswer = "Test Answer",
            Options = new List<string> { "Option1", "Option2", "Option3", "Test Answer" },
            Type = "choice",
            ThemeId = "test-theme",
            Lang = "fr"
        };
        return Ok(testData);
    }
}
