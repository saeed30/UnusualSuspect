using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.InnerModels.Game;

namespace UnusualSuspect.Services.Contracts;

public interface INotificationService
{
  Task SendSignalToGameGroup(int gameId, SignalCommands command, object? data = null);
  Task SendSignalToUser(int userId, SignalCommands command, object? data = null);
  Task NotifyOnGameStart(GameGetResponse game);
  Task AddToGroupAsync(int userId, string connectionId, string groupName);
}