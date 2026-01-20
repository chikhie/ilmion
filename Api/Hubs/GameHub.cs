using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Ilmanar.Api.Services;
using System.Security.Claims;

namespace Ilmanar.Api.Hubs;

public class GameHub : Hub
{
    private readonly ILobbyService _lobbyService;

    public GameHub(ILobbyService lobbyService)
    {
        _lobbyService = lobbyService;
    }

    public async Task<object> CreateLobby(string username)
    {
        var lobby = await _lobbyService.CreateLobbyAsync(Context.ConnectionId, username);
        await Groups.AddToGroupAsync(Context.ConnectionId, lobby.Code);

        return new { code = lobby.Code };
    }

    public async Task<object> JoinLobby(string code, string username)
    {
        var (success, lobby) = await _lobbyService.JoinLobbyAsync(code, Context.ConnectionId, username);
        if (success && lobby != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobby.Code);
            
            // Notify others
            await Clients.Group(lobby.Code).SendAsync("PlayerJoined", new { username, isHost = false });

            // Return full player list to joiner
            return new { success = true, players = lobby.Players };
        }

        return new { success = false };
    }

    public async Task LeaveLobby()
    {
        var result = await _lobbyService.LeaveLobbyAsync(Context.ConnectionId);
        if (result.Lobby != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, result.Lobby.Code);

            if (result.Disbanded)
            {
                await Clients.Group(result.Lobby.Code).SendAsync("LobbyDisbanded");
            }
            else
            {
                var username = result.Username ?? Context.User?.Identity?.Name ?? "Joueur";
                await Clients.Group(result.Lobby.Code).SendAsync("PlayerLeft", username);
            }
        }
    }

    public async Task StartGame(string code, string gameId)
    {
        // Verify host?
        var lobby = await _lobbyService.GetLobbyByCodeAsync(code);
        if (lobby != null)
        {
            if (lobby.HostConnectionId != Context.ConnectionId)
            {
                throw new HubException("Only host can start the game.");
            }

            lobby.IsGameStarted = true;
            lobby.CurrentGameId = gameId;

            await Clients.Group(code).SendAsync("GameStarted", gameId);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Graceful disconnect
        var lobby = await _lobbyService.DisconnectPlayerAsync(Context.ConnectionId);
        
        // We do NOT send PlayerLeft immediately. 
        // We could send "PlayerDisconnected" to show a greyed out icon? 
        // For now, let's keep it silent to handle page refresh seamlessly.
        
        await base.OnDisconnectedAsync(exception);
    }
}
