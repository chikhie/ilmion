using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ilmanar.Infra.Services;

public class QuizService
{
    private readonly IMongoCollection<Question> _questions;
    private readonly IMemoryCache _cache;

    public QuizService(IOptions<MongoDbSettings> settings, IMemoryCache cache)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _questions = mongoDatabase.GetCollection<Question>("Questions");
        _cache = cache;
    }

    public async Task<List<Question>> GetQuestionsForGameAsync(Guid gameId, int? count = null)
    {
        List<Question> questions;
        var cacheKey = $"questions_{gameId}";
        
        if (_cache.TryGetValue(cacheKey, out List<Question>? cachedQuestions))
        {
            questions = cachedQuestions ?? new List<Question>();
        }
        else
        {
            try 
            {
                questions = await _questions.Find(q => q.GameId == gameId).SortBy(q => q.Order).ToListAsync();
                
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(24))
                    .SetAbsoluteExpiration(TimeSpan.FromDays(7));

                _cache.Set(cacheKey, questions, cacheOptions);
            }
            catch (Exception)
            {
                questions = new List<Question>();
            }
        }

        if (count.HasValue && count.Value > 0 && questions.Count > 0)
        {
            var rnd = Random.Shared;
            return questions.OrderBy(x => rnd.Next()).Take(count.Value).ToList();
        }

        return questions;
    }
    
    public async Task CreateQuestionAsync(Question newQuestion)
    {
        await _questions.InsertOneAsync(newQuestion);
        _cache.Remove($"questions_{newQuestion.GameId}"); // Invalidate cache
    }

    public async Task RemoveQuestionsForGameAsync(Guid gameId)
    {
        await _questions.DeleteManyAsync(q => q.GameId == gameId);
        _cache.Remove($"questions_{gameId}"); // Invalidate cache
    }
}
