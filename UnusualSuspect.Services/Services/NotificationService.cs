using Microsoft.AspNetCore.SignalR;
using UnusualSuspect.ApiViewModels.Contracts;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Utilities;
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
    tasks.Add(SendSignalToGameGroup(game.GameBaseDto.Id, SignalCommands.NewGameStarted, game.GameBaseDto.Id));
    await Task.WhenAll(tasks.ToArray());
  }
  private static readonly SemaphoreSlim SemaphoreUserSignalRGroups = new SemaphoreSlim(1, 1);
  private static readonly SemaphoreSlim SemaphoreSignalRGroupOnlineUsers = new SemaphoreSlim(1, 1);

  public async Task RemoveFromAllGroupsAsync(int userId)
  {
    var groups = await memoryCacheService.GetUserSignalRGroups(userId);
    foreach (string groupName in groups)
      await RemoveFromGroupAsync(userId, groupName);
  }

  public async Task RemoveAllUsersFromGame(int gameId)
  {
    string groupName = gameId.ToString();
    var userIds = await memoryCacheService.GetSignalRGroupOnlineUsers(groupName);
    foreach (int userId in userIds)
      await RemoveFromGroupAsync(userId, groupName);
  }

  public async Task RemoveFromGroupAsync(int userId, string groupName, string? connectionId = null)
  {
    if (connectionId != null)
      await context.Groups.RemoveFromGroupAsync(connectionId, groupName);

    await SemaphoreUserSignalRGroups.WaitAsync();
    var groups = await memoryCacheService.GetUserSignalRGroups(userId);
    if (groups.Contains(groupName))
    {
      groups.Remove(groupName);
      memoryCacheService.SetUserSignalRGroups(userId, groups);
    }
    SemaphoreUserSignalRGroups.Release();

    await SemaphoreSignalRGroupOnlineUsers.WaitAsync();
    var userIds = await memoryCacheService.GetSignalRGroupOnlineUsers(groupName);
    if (userIds.Contains(userId))
    {
      userIds.Remove(userId);
      memoryCacheService.SetSignalRGroupOnlineUsers(groupName, userIds);
    }
    SemaphoreSignalRGroupOnlineUsers.Release();
  }


  public async Task AddToGroupAsync(int userId, string connectionId, string groupName)
  {
    var task = context.Groups.AddToGroupAsync(connectionId, groupName);

    await SemaphoreUserSignalRGroups.WaitAsync();
    var groups = await memoryCacheService.GetUserSignalRGroups(userId);
    if (!groups.Contains(groupName))
    {
      groups.Add(groupName);
      memoryCacheService.SetUserSignalRGroups(userId, groups);
    }
    SemaphoreUserSignalRGroups.Release();
    await SemaphoreSignalRGroupOnlineUsers.WaitAsync();
    var userIds = await memoryCacheService.GetSignalRGroupOnlineUsers(groupName);
    if (!userIds.Contains(userId))
    {
      userIds.Add(userId);
      memoryCacheService.SetSignalRGroupOnlineUsers(groupName, userIds);
    }
    SemaphoreSignalRGroupOnlineUsers.Release();

    await task;
  }
}