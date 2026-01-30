using Microsoft.AspNetCore.Mvc;
using Ilmanar.Infra.Services;
using Ilmanar.Infra.Entities.Mongo;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThemeController : ControllerBase
{
    private readonly QuizService _quizService;

    public ThemeController(QuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Theme>>> GetThemes()
    {
        var themes = await _quizService.GetThemesAsync();
        return Ok(themes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Theme>> GetTheme(string id)
    {
        var theme = await _quizService.GetThemeByIdAsync(id);
        if (theme == null) return NotFound();
        return Ok(theme);
    }

    [HttpGet("{themeId}/subjects/{subjectId}/parts/{partId}/quiz")]
    public async Task<ActionResult<IEnumerable<QuizItem>>> GetQuiz(string themeId, string subjectId, string partId, [FromQuery] int? count)
    {
        var questions = await _quizService.GetQuestionsForPartAsync(themeId, subjectId, partId, count);
        return Ok(questions);
    }
}
