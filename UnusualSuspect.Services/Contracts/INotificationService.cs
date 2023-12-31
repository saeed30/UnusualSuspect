using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.Services.Contracts;

public interface INotificationService
{
  Task SendSignalToGameGroup(int gameId, SignalCommands command, object? data = null);
  Task SendSignalToUser(int userId, SignalCommands command, object? data = null);
  Task NotifyOnGameStart(GameGetResponse game);
  Task AddToGroupAsync(int userId, string connectionId, string groupName);
  Task RemoveFromGroupAsync(int userId, string groupName, string? connectionId = null);
  Task RemoveFromAllGroupsAsync(int userId);
}