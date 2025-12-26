using Ilmanar.Api.Services;
using Microsoft.AspNetCore.SignalR;

namespace Ilmanar.Api.Hubs;

public class GameHub : Hub
{
    private readonly MultiplayerService _multiplayerService;

    public GameHub(MultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }

    public async Task<string?> CreateLobby(string username, Guid gameId)
    {
        var lobby = await _multiplayerService.CreateLobby(Context.ConnectionId, username, gameId);
        await Groups.AddToGroupAsync(Context.ConnectionId, lobby.Code);
        return lobby.Code;
    }

    public async Task JoinLobby(string code, string username)
    {
        var result = _multiplayerService.JoinLobby(code, Context.ConnectionId, username);
        if (result.Success && result.Lobby != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, result.Lobby.Code);
            // Notify others in the lobby
            await Clients.Group(result.Lobby.Code).SendAsync("PlayerJoined", result.Lobby.Players);
        }
        else
        {
            await Clients.Caller.SendAsync("Error", result.Message);
        }
    }

    public async Task StartGame(string code)
    {
        var lobby = _multiplayerService.GetLobby(code);
        if (lobby != null && lobby.HostConnectionId == Context.ConnectionId)
        {
            lobby.Status = GameStatus.Playing;
            _multiplayerService.ResetRound(code); 
            // Send questions to all clients
            await Clients.Group(code).SendAsync("GameStarted", lobby.Questions);
        }
    }
    
    public async Task UpdateScore(string code, int score)
    {
        var lobby = _multiplayerService.GetLobby(code);
        if (lobby != null)
        {
            var player = lobby.Players.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
            if (player != null)
            {
                player.Score = score;
                // Broadcast updated scores
                await Clients.Group(code).SendAsync("ScoreUpdated", lobby.Players);
            }
        }
    }
    
    public async Task SubmitAnswer(string code)
    {
         var result = _multiplayerService.SubmitAnswer(Context.ConnectionId);
         if (result.Item2 != null)
         {
             // Optional: notify that a player answered (e.g. for progress bar)
             if (result.AllAnswered)
             {
                 await Clients.Group(code).SendAsync("RevealAnswer");
             }
         }
    }
    
    public async Task NextQuestion(string code)
    {
        var lobby = _multiplayerService.GetLobby(code);
        // Only host
        if (lobby != null && lobby.HostConnectionId == Context.ConnectionId)
        {
             _multiplayerService.ResetRound(code); 
             await Clients.Group(code).SendAsync("NextQuestion");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var lobby = _multiplayerService.GetLobbyByConnectionId(Context.ConnectionId);
        if (lobby != null)
        {
            if (lobby.HostConnectionId == Context.ConnectionId)
            {
                // Host left -> Close Lobby
                _multiplayerService.RemoveLobby(lobby.Code);
                await Clients.Group(lobby.Code).SendAsync("LobbyClosed", "Le chef de caravane a quitté la partie.");
            }
            else
            {
                // Normal player left
                 _multiplayerService.RemovePlayer(Context.ConnectionId);
                 await Clients.Group(lobby.Code).SendAsync("PlayerLeft", lobby.Players);
            }
        }
        await base.OnDisconnectedAsync(exception);
    }
}
