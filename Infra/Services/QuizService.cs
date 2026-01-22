using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Ilmanar.Infra.Services;

public class QuizService
{
    private readonly List<QuizItem> _cachedQuestions;
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<QuizService> _logger;

    public QuizService(IOptions<MongoDbSettings> settings, IMemoryCache cache, IWebHostEnvironment env, ILogger<QuizService> logger)
    {
        _cache = cache;
        _env = env;
        _cachedQuestions = new List<QuizItem>();
        _logger = logger;
        LoadQuestionsFromJson();
    }

    private void LoadQuestionsFromJson()
    {
        try
        {
            var quizPath = Path.Combine(_env.ContentRootPath, "Infra", "Data", "SeedData", "culture_quiz.json");
            if (File.Exists(quizPath))
            {
                var content = File.ReadAllText(quizPath);
                using var doc = System.Text.Json.JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty("questions", out var questionsElement))
                {
                    var questions = questionsElement.EnumerateArray();
                    int order = 0;
                    foreach (var q in questions)
                    {
                        // Map JSON to QuizItem
                        var newItem = new QuizItem
                        {
                            Text = q.GetProperty("text").GetString() ?? "",
                            Type = q.GetProperty("type").GetString(),
                            CorrectAnswer = q.GetProperty("correctAnswer").GetString(),
                            Explanation = q.TryGetProperty("explanation", out var exp) ? exp.GetString() : null,
                        };

                        // Handle options
                        if (q.TryGetProperty("options", out var opts))
                        {
                            var optionsList = new List<string>();
                            foreach (var o in opts.EnumerateArray())
                            {
                                optionsList.Add(o.GetString() ?? "");
                            }
                            newItem.Options = optionsList;
                        }

                        _cachedQuestions.Add(newItem);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading quiz JSON: {ex.Message}");
        }
    }

    public async Task<List<QuizItem>> GetQuestionsForQuiz(int? count)
    {
        List<QuizItem> questions = [.. _cachedQuestions
            .OrderBy(_ => Random.Shared.Next())
            .Take(count ?? 5)];
        return await Task.FromResult(questions);
    }
}
