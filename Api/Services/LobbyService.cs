using System.Collections.Concurrent;

namespace Ilmanar.Api.Services;

public class LobbyPlayer
{
    public string ConnectionId { get; set; }
    public string Username { get; set; }
    public bool IsHost { get; set; }
}

public class Lobby
{
    public string Code { get; set; }
    public string HostConnectionId { get; set; }
    public List<LobbyPlayer> Players { get; set; } = new();
    public bool IsGameStarted { get; set; }
    public string? CurrentGameId { get; set; }
}

public interface ILobbyService
{
    Task<Lobby> CreateLobbyAsync(string connectionId, string username);
    Task<(bool Success, Lobby? Lobby)> JoinLobbyAsync(string code, string connectionId, string username);
    Task<Lobby?> LeaveLobbyAsync(string connectionId);
    Task<Lobby?> GetLobbyByCodeAsync(string code);
    Task<Lobby?> GetLobbyByConnectionIdAsync(string connectionId);
}

public class LobbyService : ILobbyService
{
    private readonly ConcurrentDictionary<string, Lobby> _lobbies = new(); // Key: Code
    private readonly ConcurrentDictionary<string, string> _playerLobbyMap = new(); // Key: ConnectionId, Value: Code

    public Task<Lobby> CreateLobbyAsync(string connectionId, string username)
    {
        var code = GenerateCode();
        var lobby = new Lobby
        {
            Code = code,
            HostConnectionId = connectionId,
            Players = new List<LobbyPlayer>
            {
                new LobbyPlayer { ConnectionId = connectionId, Username = username, IsHost = true }
            }
        };

        _lobbies.TryAdd(code, lobby);
        _playerLobbyMap.TryAdd(connectionId, code);

        return Task.FromResult(lobby);
    }

    public Task<(bool Success, Lobby? Lobby)> JoinLobbyAsync(string code, string connectionId, string username)
    {
        if (_lobbies.TryGetValue(code.ToUpper(), out var lobby))
        {
            if (lobby.IsGameStarted) return Task.FromResult((false, (Lobby?)null)); // Or allow join in progress? For now no.

            // Prevent duplicate join if connection already exists (though specialized handling might be needed)
            if (lobby.Players.Any(p => p.ConnectionId == connectionId))
                return Task.FromResult((true, (Lobby?)lobby));

            var player = new LobbyPlayer { ConnectionId = connectionId, Username = username, IsHost = false };
            lobby.Players.Add(player);
            _playerLobbyMap.TryAdd(connectionId, code);

            return Task.FromResult((true, (Lobby?)lobby));
        }

        return Task.FromResult((false, (Lobby?)null));
    }

    public Task<Lobby?> LeaveLobbyAsync(string connectionId)
    {
        if (_playerLobbyMap.TryRemove(connectionId, out var code))
        {
            if (_lobbies.TryGetValue(code, out var lobby))
            {
                var player = lobby.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
                if (player != null)
                {
                    lobby.Players.Remove(player);
                    
                    // If host left, assign new host or destroy lobby?
                    // Simple logic: if empty, destroy. If host left, promote next or destroy.
                    if (lobby.Players.Count == 0)
                    {
                        _lobbies.TryRemove(code, out _);
                        return Task.FromResult<Lobby?>(lobby); // Return lobby to notify emptiness if needed
                    }
                    else if (player.IsHost)
                    {
                        lobby.Players[0].IsHost = true;
                        lobby.HostConnectionId = lobby.Players[0].ConnectionId;
                        // TODO: Notify new host?
                    }
                    
                    return Task.FromResult<Lobby?>(lobby);
                }
            }
        }
        return Task.FromResult<Lobby?>(null);
    }

    public Task<Lobby?> GetLobbyByCodeAsync(string code)
    {
        _lobbies.TryGetValue(code.ToUpper(), out var lobby);
        return Task.FromResult(lobby);
    }

    public Task<Lobby?> GetLobbyByConnectionIdAsync(string connectionId)
    {
        if (_playerLobbyMap.TryGetValue(connectionId, out var code))
        {
            _lobbies.TryGetValue(code, out var lobby);
            return Task.FromResult(lobby);
        }
        return Task.FromResult<Lobby?>(null);
    }

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
