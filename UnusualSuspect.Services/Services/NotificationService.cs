using Microsoft.AspNetCore.SignalR;
using UnusualSuspect.ApiViewModels.Contracts;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.SignalR;

namespace UnusualSuspect.Services.Services;

public sealed class NotificationService(IHubContext<GameHub, IGameClient> context,
  IMemoryCacheService memoryCacheService) : INotificationService
{
  public async Task SendSignalToGameGroup(int gameId, SignalCommands command, object? data = null)
  {
    await context.Clients.Group(gameId.ToString()).GameCommand(command, data);
  }

  public async Task SendSignalToUser(int userId, SignalCommands command, object? data = null)
  {
    await context.Clients.User(userId.ToString()).GameCommand(command, data);
  }

  public async Task NotifyOnGameStart(GameGetResponse game)
  {
    List<Task> tasks = new List<Task>();
    foreach (var gameParticipantDto in game.GameBaseDto.GameParticipantDto)
    {
      var connections = await memoryCacheService.GetUserSignalRConnections(gameParticipantDto.GameUserDto.Id);
      foreach (var connection in connections)
        tasks.Add(AddToGroupAsync(gameParticipantDto.GameUserDto.Id, connection, game.GameBaseDto.Id.ToString()));
    }
    await Task.WhenAll(tasks.ToArray());
    await SendSignalToGameGroup(game.GameBaseDto.Id,SignalCommands.NewGameStarted, game.GameBaseDto.Id);
  }

  public async Task AddToGroupAsync(int userId, string connectionId, string groupName)
  {
    var task = context.Groups.AddToGroupAsync(connectionId, groupName);
    var groups = await memoryCacheService.GetUserSignalRGroups(userId);
    if (!groups.Contains(groupName))
      groups.Add(groupName);
    memoryCacheService.SetUserSignalRGroups(userId, groups);
    await task;
  }
}