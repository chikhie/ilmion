using System.Collections.Concurrent;
using System.Text.Json;
using Ilmanar.Infra;
using Microsoft.EntityFrameworkCore;

namespace Ilmanar.Api.Services;

public class MultiplayerService
{
    private readonly ConcurrentDictionary<string, Lobby> _lobbies = new();
    private readonly IServiceScopeFactory _scopeFactory;

    public MultiplayerService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<Lobby> CreateLobby(string hostConnectionId, string username, Guid gameId)
    {
        var code = GenerateLobbyCode();
        var lobby = new Lobby
        {
            Code = code,
            GameId = gameId,
            HostConnectionId = hostConnectionId,
            Status = GameStatus.Waiting
        };

        // Fetch Questions
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var game = await db.Games.FindAsync(gameId);
            if (game != null && !string.IsNullOrEmpty(game.ContentJson))
            {
                try 
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var gameContent = JsonSerializer.Deserialize<GameContentModel>(game.ContentJson, options);
                    if(gameContent?.Questions != null)
                    {
                        lobby.Questions = gameContent.Questions;
                        Console.WriteLine($"[Lobby {code}] Loaded {lobby.Questions.Count} questions.");
                        // Optional: Shuffle here if desired
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error parsing game content: {ex.Message}");
                }
            }
        }

        lobby.Players.Add(new Player { ConnectionId = hostConnectionId, Username = username });
        _lobbies.TryAdd(code, lobby);
        
        return lobby;
    }

    public (bool Success, string Message, Lobby? Lobby) JoinLobby(string code, string connectionId, string username)
    {
        if (!_lobbies.TryGetValue(code.ToUpper(), out var lobby))
        {
            return (false, "Lobby introuvable.", null);
        }

        if (lobby.Status != GameStatus.Waiting)
        {
            return (false, "La partie a déjà commencé.", null);
        }

        if (lobby.Players.Any(p => p.Username == username))
        {
             username = $"{username}{new Random().Next(100, 999)}";
        }

        var player = new Player { ConnectionId = connectionId, Username = username };
        lobby.Players.Add(player);
        
        Console.WriteLine($"[JoinLobby] Player {username} joined lobby {code}. Total Players: {lobby.Players.Count}");

        return (true, "Rejoint avec succès.", lobby);
    }

    public Lobby? GetLobby(string code)
    {
        _lobbies.TryGetValue(code.ToUpper(), out var lobby);
        return lobby;
    }
    
    public Lobby? GetLobbyByConnectionId(string connectionId)
    {
        return _lobbies.Values.FirstOrDefault(l => l.Players.Any(p => p.ConnectionId == connectionId));
    }

    public void RemovePlayer(string connectionId)
    {
        var lobby = GetLobbyByConnectionId(connectionId);
        if (lobby != null)
        {
            var player = lobby.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player != null)
            {
                lobby.Players.Remove(player);
                if (lobby.Players.Count == 0)
                {
                    _lobbies.TryRemove(lobby.Code, out _);
                }
                else if (lobby.HostConnectionId == connectionId)
                {
                    lobby.HostConnectionId = lobby.Players.First().ConnectionId;
                }
            }
        }
    }

    public void RemoveLobby(string code)
    {
        _lobbies.TryRemove(code.ToUpper(), out _);
    }
    
    // Synch Methods
    public (bool AllAnswered, Lobby? Lobby) SubmitAnswer(string connectionId)
    {
        var lobby = GetLobbyByConnectionId(connectionId);
        if (lobby == null) 
        {
             Console.WriteLine($"[SubmitAnswer] Lobby not found for {connectionId}");
             return (false, null);
        }

        var player = lobby.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
        if (player != null)
        {
             player.HasAnswered = true;
             Console.WriteLine($"[SubmitAnswer] Player {player.Username} answered. ({lobby.Players.Count(p => p.HasAnswered)}/{lobby.Players.Count})");
        }
        
        bool all = lobby.Players.All(p => p.HasAnswered);
        if(all) Console.WriteLine($"[SubmitAnswer] ALL PLAYERS ANSWERED. Triggering Reveal.");
        
        return (all, lobby);
    }
    
    public void ResetRound(string code)
    {
        var lobby = GetLobby(code);
        if(lobby != null)
        {
            // Reset pending answers count to player count
            lobby.PendingAnswers = lobby.Players.Count;
            foreach(var p in lobby.Players) p.HasAnswered = false;
        }
    }

    private string GenerateLobbyCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

public class Lobby
{
    public string Code { get; set; } = string.Empty;
    public Guid GameId { get; set; }
    public string HostConnectionId { get; set; } = string.Empty;
    public List<Player> Players { get; set; } = new();
    public List<QuestionModel> Questions { get; set; } = new();
    public GameStatus Status { get; set; } = GameStatus.Waiting;
    public int PendingAnswers { get; set; } = 0;
}

public class Player
{
    public string ConnectionId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int Score { get; set; } = 0;
    public bool HasAnswered { get; set; } = false;
}

public enum GameStatus
{
    Waiting,
    Playing,
    Finished
}

// Models for JSON parsing
public class GameContentModel
{
    public List<QuestionModel> Questions { get; set; } = new();
}

public class QuestionModel
{
    public string Type { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public string CorrectAnswer { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
}
