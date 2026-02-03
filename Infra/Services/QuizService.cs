using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ilmanar.Infra.Services;

public class QuizService
{
    private readonly IMongoCollection<Theme> _themeCollection;
    private readonly IMongoCollection<QuizItem> _quizCollection;
    private readonly IMemoryCache _cache;
    private readonly ILogger<QuizService> _logger;

    public QuizService(IOptions<MongoDbSettings> settings, IMemoryCache cache, ILogger<QuizService> logger)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _themeCollection = database.GetCollection<Theme>("Themes");
        _quizCollection = database.GetCollection<QuizItem>("QuizItems");
        _cache = cache;
        _logger = logger;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<Theme>> GetThemesAsync()
    {
        return await _themeCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Theme?> GetThemeByIdAsync(string id)
    {
        return await _themeCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<QuizItem>> GetQuestionsForPartAsync(string themeId, string subjectId, string partId, int? count)
    {
        var filter = Builders<QuizItem>.Filter.And(
            Builders<QuizItem>.Filter.Eq(q => q.ThemeId, themeId),
            Builders<QuizItem>.Filter.Eq(q => q.SubjectId, subjectId),
            Builders<QuizItem>.Filter.Eq(q => q.PartId, partId)
        );

        var questions = await _quizCollection.Find(filter).ToListAsync();
        
        if (count.HasValue && count.Value < questions.Count)
        {
            return questions.OrderBy(_ => Random.Shared.Next()).Take(count.Value).ToList();
        }

        return questions;
    }

    public async Task<long> GetTotalQuestionsCount()
    {
        return await _quizCollection.CountDocumentsAsync(_ => true);
    }

    // Creating this for backward compatibility with LobbyService
    // It flattens all questions from all themes/subjects/parts
    // Updated to support filtering by Theme OR Part
    public async Task<List<QuizItem>> GetQuestionsForQuiz(int? count, string? lang = "fr", string? themeId = null, string? partId = null)
    {
        var builder = Builders<QuizItem>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(themeId))
        {
            filter &= builder.Eq(q => q.ThemeId, themeId);
        }
        
        if (!string.IsNullOrEmpty(partId))
        {
            filter &= builder.Eq(q => q.PartId, partId);
        }

        var allQuestions = await _quizCollection.Find(filter).ToListAsync();

        if (count.HasValue && count.Value < allQuestions.Count)
        {
            return allQuestions.OrderBy(_ => Random.Shared.Next()).Take(count.Value).ToList();
        }

        return allQuestions;
    }
}
