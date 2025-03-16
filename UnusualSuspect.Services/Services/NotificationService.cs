using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Contracts;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.SignalR;

namespace UnusualSuspect.Services.Services;

public sealed class NotificationService(IHubContext<GameHub, IGameClient> context,
  IMemoryCacheService memoryCacheService,
  ILogger<NotificationService> logger) : INotificationService
{
  private string GetPreGameGroupName(int preGameGroupId)
  {
    return "pre" + preGameGroupId;
  }

  public async Task SendSignalToPreGameGroup(int preGameGroupId, SignalCommands command, object? data = null)
  {
    logger.LogEvent(SystemEventType.SendSignalToPreGameGroup, preGameGroupId, command.ToString());
    await context.Clients.Group(GetPreGameGroupName(preGameGroupId)).GameCommand(command, data);
  }

  public async Task SendSignalToGameGroup(int gameId, SignalCommands command, object? data = null)
  {
    logger.LogEvent(SystemEventType.SendSignalToGameGroup, gameId, command.ToString());
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
      var connections = await memoryCacheService.GetUserSignalRConnections(gameParticipantDto.UserDto.Id);
      foreach (var connection in connections)
        tasks.Add(AddToGroupAsync(gameParticipantDto.UserDto.Id, connection, game.GameBaseDto.Id.ToString()));
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
    if (!string.IsNullOrWhiteSpace(connectionId))
      await context.Groups.RemoveFromGroupAsync(connectionId, groupName);

    await SemaphoreUserSignalRGroups.WaitAsync();
    var groups = await memoryCacheService.GetUserSignalRGroups(userId);
    if (groups.Contains(groupName))
      memoryCacheService.SetUserSignalRGroups(userId, groups.Where(x => x != groupName));
    SemaphoreUserSignalRGroups.Release();

    await SemaphoreSignalRGroupOnlineUsers.WaitAsync();
    var userIds = await memoryCacheService.GetSignalRGroupOnlineUsers(groupName);
    if (userIds.Contains(userId))
      memoryCacheService.SetSignalRGroupOnlineUsers(groupName, userIds.Where(x => x != userId));
    SemaphoreSignalRGroupOnlineUsers.Release();
  }


  public async Task AddToGroupAsync(int userId, string connectionId, string groupName)
  {
    if (!string.IsNullOrWhiteSpace(connectionId))
      await context.Groups.AddToGroupAsync(connectionId, groupName);

    await SemaphoreUserSignalRGroups.WaitAsync();
    var groups = await memoryCacheService.GetUserSignalRGroups(userId);
    if (!groups.Contains(groupName))
      memoryCacheService.SetUserSignalRGroups(userId, groups.Concat([groupName]));
    SemaphoreUserSignalRGroups.Release();
    await SemaphoreSignalRGroupOnlineUsers.WaitAsync();
    var userIds = await memoryCacheService.GetSignalRGroupOnlineUsers(groupName);
    if (!userIds.Contains(userId))
      memoryCacheService.SetSignalRGroupOnlineUsers(groupName, userIds.Concat([userId]));
    SemaphoreSignalRGroupOnlineUsers.Release();
  }
}