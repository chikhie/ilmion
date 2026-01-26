using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Ilmanar.Infra.Services;

public class QuizService
{
    private readonly Dictionary<string, List<QuizItem>> _questionsByLang;
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<QuizService> _logger;

    private static readonly string[] SupportedLanguages = ["fr", "en"];

    public QuizService(IOptions<MongoDbSettings> settings, IMemoryCache cache, IWebHostEnvironment env, ILogger<QuizService> logger)
    {
        _cache = cache;
        _env = env;
        _questionsByLang = new Dictionary<string, List<QuizItem>>();
        _logger = logger;
        LoadAllQuestions();
    }

    private void LoadAllQuestions()
    {
        foreach (var lang in SupportedLanguages)
        {
            _questionsByLang[lang] = LoadQuestionsFromJson(lang);
        }
    }

    private List<QuizItem> LoadQuestionsFromJson(string lang)
    {
        var questions = new List<QuizItem>();
        try
        {
            var quizPath = Path.Combine(_env.ContentRootPath, "Infra", "Data", "SeedData", $"culture_quiz_{lang}.json");
            if (File.Exists(quizPath))
            {
                var content = File.ReadAllText(quizPath);
                using var doc = System.Text.Json.JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty("questions", out var questionsElement))
                {
                    foreach (var q in questionsElement.EnumerateArray())
                    {
                        var newItem = new QuizItem
                        {
                            Text = q.GetProperty("text").GetString() ?? "",
                            Type = q.GetProperty("type").GetString(),
                            Lang = lang,
                            CorrectAnswer = q.GetProperty("correctAnswer").GetString(),
                            Explanation = q.TryGetProperty("explanation", out var exp) ? exp.GetString() : null,
                        };

                        if (q.TryGetProperty("options", out var opts))
                        {
                            var optionsList = new List<string>();
                            foreach (var o in opts.EnumerateArray())
                            {
                                optionsList.Add(o.GetString() ?? "");
                            }
                            newItem.Options = optionsList;
                        }

                        questions.Add(newItem);
                    }
                }
                _logger.LogInformation("Loaded {Count} questions for language '{Lang}'", questions.Count, lang);
            }
            else
            {
                _logger.LogWarning("Quiz file not found for language '{Lang}': {Path}", lang, quizPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading quiz JSON for language '{Lang}'", lang);
        }
        return questions;
    }

    public async Task<List<QuizItem>> GetQuestionsForQuiz(int? count, string? lang = "fr")
    {
        var language = lang ?? "fr";
        if (!_questionsByLang.TryGetValue(language, out var availableQuestions))
        {
            availableQuestions = _questionsByLang.GetValueOrDefault("fr", []);
        }

        List<QuizItem> questions = [.. availableQuestions
            .OrderBy(_ => Random.Shared.Next())
            .Take(count ?? 5)];
        
        return await Task.FromResult(questions);
    }
}
