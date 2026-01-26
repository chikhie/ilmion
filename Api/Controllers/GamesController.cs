using Ilmanar.Api.Dtos;
using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly QuizService _quizService;

    public GamesController(QuizService quizService)
    {
        _quizService = quizService;
    }


    [HttpGet("quizzes")]
    public async Task<ActionResult<List<QuizItem>>> GetQuizzes([FromQuery] int? count, [FromQuery] string? lang = "fr")
    {
        var quizzes = await _quizService.GetQuestionsForQuiz(count, lang);
        return Ok(quizzes);
    }

}
