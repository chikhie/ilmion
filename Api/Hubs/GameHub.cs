using System.Security.Claims;
using Ilmanar.Api.Services;
using Ilmanar.Infra.Entities.Mongo;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

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

    public async Task SubmitAnswer(string code, string answer)
    {
        var (allAnswered, lobby) = await _lobbyService.SubmitAnswerAsync(code, Context.ConnectionId, answer);
        if (lobby != null && allAnswered)
        {
            // If all players have answered, signal to reveal results and send updated scores
            var players = lobby.Players.Select(p => new { p.Username, p.Score, p.ConnectionId, p.IsHost }).ToList();
            await Clients.Group(code).SendAsync("ShowReveal", players);
        }
    }

    public async Task RequestNextQuestion(string code)
    {
        // Only host should call this technically, but checks could be loose for now or strictly by host connection
        var lobby = await _lobbyService.GetLobbyByCodeAsync(code);
        if (lobby != null && lobby.HostConnectionId == Context.ConnectionId)
        {
            await _lobbyService.NextQuestionAsync(code);
            await Clients.Group(code).SendAsync("NextQuestion", lobby.CurrentQuestionIndex);
        }
    }

    public async Task StartGame(string code, string gameId, object? options = null)
    {
        var (success, lobby) = await _lobbyService.StartGameAsync(code, Context.ConnectionId, gameId, options);

        if (success && lobby != null)
        {
            // Project each item to a safe object based on its actual type
            var items = lobby.Questions.Select(item =>
            {
                if (item is QuizItem quizItem)
                {
                    return (object)new
                    {
                        quizItem.Text,
                        quizItem.Type,
                        quizItem.CorrectAnswer,
                        quizItem.Options,
                        quizItem.Explanation,
                    };
                }

                return (object)new
                {
                    item.Text,
                    item.Type,
                    item.CorrectAnswer,
                    item.Options,
                    item.Explanation,
                };
            }).ToList();

            await Clients.Group(code).SendAsync("GameStarted", gameId, options, items);
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
