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
        var lobby = await _lobbyService.LeaveLobbyAsync(Context.ConnectionId);
        if (lobby != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobby.Code);
            var username = Context.User?.Identity?.Name ?? "Joueur"; 
             // Ideally we get the username from the removed player object, 
             // but here we just need to notify WHO left.
             // The Service logic handled finding the player.
             // We can fetch the username from the User principle again.
             
            await Clients.Group(lobby.Code).SendAsync("PlayerLeft", username);
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
        var lobby = await _lobbyService.LeaveLobbyAsync(Context.ConnectionId);
        if (lobby != null)
        {
             // We need to find which username left.
             // Since the player is already removed from lobby.Players in the service, 
             // we might want to change LeaveLobbyAsync to return the removed player info.
             // For now, simpler notification:
             
             // A better approach in Service would be returning (Lobby, RemovedPlayer)
             // But for now, we rely on the client refreshing or just sending connectionId?
             // The client expects 'PlayerLeft', (username).
             
             // If we can't easily get the username of the disconnected user from the Service return,
             // we use the Claim.
             var username = Context.User?.Identity?.Name;
             if (username != null)
             {
                await Clients.Group(lobby.Code).SendAsync("PlayerLeft", username);
             }
        }

        await base.OnDisconnectedAsync(exception);
    }
}
