using Ilmanar.Api.Models.Mongo;
using Ilmanar.Infra.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ilmanar.Api.Services;

public class QuizService
{
    private readonly IMongoCollection<Question> _questions;

    public QuizService(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _questions = mongoDatabase.GetCollection<Question>("Questions");
    }

    public async Task<List<Question>> GetQuestionsForGameAsync(Guid gameId)
    {
        try 
        {
            return await _questions.Find(q => q.GameId == gameId).SortBy(q => q.Order).ToListAsync();
        }
        catch (Exception)
        {
            // Fallback for dev if Mongo not running or empty
            return new List<Question>();
        }
    }
    
    // Helper to seed or add questions if needed (future use)
    public async Task CreateQuestionAsync(Question newQuestion) =>
        await _questions.InsertOneAsync(newQuestion);
}
