using System.Collections.Concurrent;
using System.Text.Json;
using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Services;

namespace Ilmanar.Api.Services;

public class LobbyPlayer
{
    public required string ConnectionId { get; set; }
    public required string Username { get; set; }
    public bool IsHost { get; set; }
    public bool IsConnected { get; set; } = true;
    public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    public int Score { get; set; } = 0;
}

public class Lobby
{
    public required string Code { get; set; }
    public required string HostConnectionId { get; set; }
    public List<LobbyPlayer> Players { get; set; } = [];
    public bool IsGameStarted { get; set; }
    public string? CurrentGameId { get; set; }
    public int CurrentQuestionIndex { get; set; } = 0;
    // Map ConnectionId -> Answer
    public Dictionary<string, string> Answers { get; set; } = [];
    public List<QuizItem> Questions { get; set; } = [];
}

public interface ILobbyService
{
    Task<Lobby> CreateLobbyAsync(string connectionId, string username);
    Task<(bool Success, Lobby? Lobby)> JoinLobbyAsync(string code, string connectionId, string username);
    Task<(Lobby? Lobby, bool Disbanded, string? Username)> LeaveLobbyAsync(string connectionId);
    Task<Lobby?> GetLobbyByCodeAsync(string code);
    Task<Lobby?> DisconnectPlayerAsync(string connectionId);
    Task<(bool AllAnswered, Lobby? Lobby)> SubmitAnswerAsync(string code, string connectionId, string answer);

    Task<Lobby?> NextQuestionAsync(string code);
    Task<(bool Success, Lobby? Lobby)> StartGameAsync(string code, string connectionId, string gameId, object? options);
}

public class LobbyService : ILobbyService
{
    private readonly ConcurrentDictionary<string, Lobby> _lobbies = new(); // Key: Code
    private readonly ConcurrentDictionary<string, string> _playerLobbyMap = new(); // Key: ConnectionId, Value: Code
    private readonly QuizService _quizService;

    public LobbyService(QuizService quizService)
    {
        _quizService = quizService;
    }

    public Task<Lobby> CreateLobbyAsync(string connectionId, string username)
    {
        var code = GenerateCode();
        var lobby = new Lobby
        {
            Code = code,
            HostConnectionId = connectionId,
            Players =
            [
                new LobbyPlayer { ConnectionId = connectionId, Username = username, IsHost = true }
            ]
        };

        _lobbies.TryAdd(code, lobby);
        _playerLobbyMap.TryAdd(connectionId, code);

        return Task.FromResult(lobby);
    }

    public Task<(bool Success, Lobby? Lobby)> JoinLobbyAsync(string code, string connectionId, string username)
    {
        if (_lobbies.TryGetValue(code.ToUpper(), out var lobby))
        {
            var existingPlayer = lobby.Players.FirstOrDefault(p => p.Username == username);
            if (existingPlayer != null)
            {
                // Reconnection attempt
                if (existingPlayer.ConnectionId != connectionId)
                {
                    // Update connection map
                    _playerLobbyMap.TryRemove(existingPlayer.ConnectionId, out _);
                    _playerLobbyMap.TryAdd(connectionId, code);

                    existingPlayer.ConnectionId = connectionId;
                }

                existingPlayer.IsConnected = true;
                existingPlayer.LastActivity = DateTime.UtcNow;

                return Task.FromResult((true, (Lobby?)lobby));
            }

            if (lobby.IsGameStarted) return Task.FromResult((false, (Lobby?)null));

            // Prevent duplicate join if connection already exists
            if (lobby.Players.Any(p => p.ConnectionId == connectionId))
                return Task.FromResult((true, (Lobby?)lobby));

            var player = new LobbyPlayer { ConnectionId = connectionId, Username = username, IsHost = false };
            lobby.Players.Add(player);
            _playerLobbyMap.TryAdd(connectionId, code);

            return Task.FromResult((true, (Lobby?)lobby));
        }

        return Task.FromResult((false, (Lobby?)null));
    }

    public async Task<Lobby?> DisconnectPlayerAsync(string connectionId)
    {
        if (_playerLobbyMap.TryGetValue(connectionId, out var code))
        {
            if (_lobbies.TryGetValue(code, out var lobby))
            {
                var player = lobby.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
                if (player != null)
                {
                    player.IsConnected = false;

                    // Fire and forget delay check
                    _ = Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromSeconds(15));
                        if (!player.IsConnected && lobby.Players.Contains(player))
                        {
                            // Re-check lobby existence
                            if (_lobbies.TryGetValue(code, out var currentLobby))
                            {
                                if (!player.IsConnected)
                                {
                                    currentLobby.Players.Remove(player);
                                    _playerLobbyMap.TryRemove(connectionId, out _);

                                    if (currentLobby.Players.Count == 0 || player.IsHost)
                                    {
                                        _lobbies.TryRemove(code, out _);
                                    }
                                }
                            }
                        }
                    });

                    return lobby;
                }
            }
        }
        return null;
    }

    public Task<(Lobby? Lobby, bool Disbanded, string? Username)> LeaveLobbyAsync(string connectionId)
    {
        if (_playerLobbyMap.TryRemove(connectionId, out var code))
        {
            if (_lobbies.TryGetValue(code, out var lobby))
            {
                var player = lobby.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
                if (player != null)
                {
                    lobby.Players.Remove(player);

                    if (player.IsHost || lobby.Players.Count == 0)
                    {
                        _lobbies.TryRemove(code, out _);
                        return Task.FromResult<(Lobby?, bool, string?)>((lobby, true, player.Username));
                    }

                    return Task.FromResult<(Lobby?, bool, string?)>((lobby, false, player.Username));
                }
            }
        }
        return Task.FromResult<(Lobby?, bool, string?)>((null, false, null));
    }

    public Task<Lobby?> GetLobbyByCodeAsync(string code)
    {
        _lobbies.TryGetValue(code.ToUpper(), out var lobby);
        return Task.FromResult(lobby);
    }

    public Task<(bool AllAnswered, Lobby? Lobby)> SubmitAnswerAsync(string code, string connectionId, string answer)
    {
        if (_lobbies.TryGetValue(code, out var lobby))
        {
            // Update answer
            if (lobby.Answers.ContainsKey(connectionId))
            {
                lobby.Answers[connectionId] = answer;
            }
            else
            {
                lobby.Answers.Add(connectionId, answer);
            }

            // Validate Answer and Update Score
            // We only support QuizItem validation for now
            if (lobby.CurrentQuestionIndex < lobby.Questions.Count)
            {
                var question = lobby.Questions[lobby.CurrentQuestionIndex];
                if (question is QuizItem quizItem)
                {
                    // Simple case-insensitive check
                    var isCorrect = string.Equals(answer.Trim(), quizItem.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase);

                    var player = lobby.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
                    if (player != null && isCorrect)
                    {
                    }
                }
            }

            // Check if all connected players have answered
            var connectedPlayersCount = lobby.Players.Count(p => p.IsConnected);
            bool allAnswered = lobby.Answers.Count >= connectedPlayersCount;

            // If all answered (or forced), we can compute scores based on final answers
            if (allAnswered)
            {
                foreach (var p in lobby.Players)
                {
                    if (lobby.Answers.TryGetValue(p.ConnectionId, out var pAns))
                    {
                        if (lobby.CurrentQuestionIndex < lobby.Questions.Count)
                        {
                            var q = lobby.Questions[lobby.CurrentQuestionIndex];
                            if (q is QuizItem qi)
                            {
                                if (string.Equals(pAns.Trim(), qi.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase))
                                {
                                    p.Score += 1; // Increment score
                                }
                            }
                        }
                    }
                }
            }

            return Task.FromResult((allAnswered, (Lobby?)lobby));
        }
        return Task.FromResult((false, (Lobby?)null));
    }

    public Task<Lobby?> NextQuestionAsync(string code)
    {
        if (_lobbies.TryGetValue(code, out var lobby))
        {
            lobby.CurrentQuestionIndex++;
            lobby.Answers.Clear();
            return Task.FromResult((Lobby?)lobby);
        }
        return Task.FromResult<Lobby?>(null);
    }

    public async Task<(bool Success, Lobby? Lobby)> StartGameAsync(string code, string connectionId, string gameId, object? options)
    {
        if (_lobbies.TryGetValue(code, out var lobby))
        {
            if (lobby.HostConnectionId != connectionId)
            {
                return (false, null);
            }

            lobby.IsGameStarted = true;
            lobby.CurrentGameId = gameId;

            int? count = null;
            if (options is JsonElement element && element.TryGetProperty("questionCount", out var prop) && prop.TryGetInt32(out var c))
            {
                count = c;
            }

            lobby.Questions = await _quizService.GetQuestionsForQuiz(count);

            return (true, lobby);
        }
        return (false, null);
    }

    // Disconnect method removed duplicate

    private string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        string code;
        do
        {
            code = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        } while (_lobbies.ContainsKey(code));
        return code;
    }
}
