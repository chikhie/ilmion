using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Settings;
using Microsoft.Extensions.Options;

namespace Ilmanar.Infra.Services;

public class GameService
{
    private readonly List<Game> _games;

    public GameService(IOptions<MongoDbSettings> settings)
    {
        // Settings kept for signature compatibility but ignored for now, or could be removed if we remove from DI
        _games = new List<Game>
        {
            new Game
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Quiz Culture Générale",
                Description = "Testez vos connaissances sur la culture islamique et l'histoire de l'Andalousie.",
                Type = GameType.Quiz,
                ThumbnailPath = "https://pixabay.com/fr/images/download/x-2492009_1920.jpg",
                CreatedAt = DateTime.UtcNow
            },
            new Game
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Exploration d'Al-Andalus",
                Description = "Placez les villes historiques d'Al-Andalus sur la carte.",
                Type = GameType.MapPlacement,
                ThumbnailPath = "",
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    public async Task<List<Game>> GetGamesByTypeAsync(GameType type)
    {
        return await Task.FromResult(_games.Where(g => g.Type == type).ToList());
    }

    public async Task CreateGameAsync(Game game)
    {
        var existing = _games.FirstOrDefault(g => g.Id == game.Id);
        if (existing == null)
        {
            _games.Add(game);
        }
        await Task.CompletedTask;
    }

    public async Task DeleteGameAsync(Guid id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game != null)
        {
            _games.Remove(game);
        }
        await Task.CompletedTask;
    }

    // For seeding checks
    public async Task<Game?> GetGameByTitleAsync(string title)
    {
        return await Task.FromResult(_games.FirstOrDefault(g => g.Title == title));
    }
}
