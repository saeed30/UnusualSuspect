using System.Security.Claims;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnusualSuspect.ApiViewModels.Contracts;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Timer;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.SignalR;

[Authorize]
public sealed class GameHub(IGameService gameService,
  IParticipateRepository participateRepository,
  INotificationService notificationService,
  IOptionsSnapshot<ProjectSetting> setting,
  IMemoryCacheService memoryCacheService,
  ITimerManagementService timerManagementService,
  IStickerService stickerService,
  ILogger<GameHub> logger) : Hub<IGameClient>, IGameHub
{
  #region Properties
  private int? UserId
  {
    get
    {
      if (Context.User == null || Context.User.Identity == null ||
         !Context.User.Identity.IsAuthenticated || Context.User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
        return null;
      string? user = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (user == null)
        return null;
      return user.ToInt();
    }
  }
  #endregion Properties

  #region PublicMethods
  public async Task SendMessage(string user, string message)
  {
    try
    {
      logger.LogEvent(SystemEventType.GameHubSendMessage,UserId,$"user ({user}) - message ({message})");
      await Clients.All.ReceiveMessage(user, message);
      if (message.StartsWith("startChooseCardTimer") && setting.Value.IsTesting)
        timerManagementService.OnGameTimerStart(message.Split(":")[1].ToInt(), GameTimerEnum.AutoChooseCard);
    }
    catch (Exception exception)
    {
      ElmahExtensions.RaiseError(exception);
      await Clients.Caller.ReceiveMessage("errorSendMessage", exception.Message);
      throw;
    }
  }
  //private void LogUserCall(string methodName, string param1, string param2)
  //{
  //  LogEvent
  //  logger.LogInformation("UserId ({UserId}) called signalR method ({methodName}) with these parameters: ({param1} - {param2})",
  //    UserId, methodName, param1, param2);
  //}
  public async Task StartedToTalk(int gameId)
  {
    try
    {
      logger.LogEvent(SystemEventType.GameHubStartedToTalk, UserId, gameId.ToString());
      await Clients.Caller.ReceiveMessage("admin", "you called StartedToTalk");
    }
    catch (Exception exception)
    {
      ElmahExtensions.RaiseError(exception);
      await Clients.Caller.ReceiveMessage("errorReceiveMessage", exception.Message);
      throw;
    }
  }

  public async Task FinishedTalking(int gameId)
  {
    try
    {
      if (!UserId.HasValue)
        return;
      logger.LogEvent(SystemEventType.GameHubFinishedTalking, UserId, gameId.ToString());
      await Clients.Caller.ReceiveMessage("admin", "you called FinishedTalking");
      TimerManagementService.OnGameTimerStop(gameId);
      var result = await gameService.UserTurnFinishedAsync(UserId.Value, gameId);
      if (!result.Success)
        await Clients.Caller.ReceiveMessage("error", result.MainError.ToString());
    }
    catch (Exception exception)
    {
      ElmahExtensions.RaiseError(exception);
      await Clients.Caller.ReceiveMessage("errorFinishedTalking", exception.Message);
      throw;
    }
  }

  public async Task UseSticker(int stickerId, int gameId)
  {
    try
    {
      if (!UserId.HasValue)
        return;
      logger.LogEvent(SystemEventType.GameHubUseSticker, UserId,$"stickerId ({stickerId}) - gameId ({gameId})");
      await Clients.Caller.ReceiveMessage("admin", "you called UseSticker");
      var result = await stickerService.SendStickerToGroupAsync(UserId.Value, (short)stickerId, gameId);
      if (!result.Success)
        await Clients.Caller.ReceiveMessage("error", result.MainError.ToString());
    }
    catch (Exception exception)
    {
      ElmahExtensions.RaiseError(exception);
      await Clients.Caller.ReceiveMessage("errorUseSticker", exception.Message);
      throw;
    }

  }
  public async Task CandidateCard(int? cardId, int gameId)
  {
    try
    {
      if (!UserId.HasValue)
        return;
      logger.LogEvent(SystemEventType.GameHubCandidateCard, UserId,$"cardId ({cardId}) - gameId ({gameId})");
      await Clients.Caller.ReceiveMessage("admin", "you called CandidateCard");
      var result = await gameService.ChangedCandidateCard(UserId.Value, (short?)cardId, gameId);
      if (!result.Success)
        await Clients.Caller.ReceiveMessage("error", result.MainError.ToString());
    }
    catch (Exception exception)
    {
      ElmahExtensions.RaiseError(exception);
      await Clients.Caller.ReceiveMessage("errorCandidateCard", exception.Message);
      throw;
    }
  }

  #endregion PublicMethods

  #region Events
  public override async Task OnConnectedAsync()
  {
    try
    {
      logger.LogEvent(SystemEventType.GameHubOnConnectedAsync, UserId, Context.ConnectionId);
      int? userId = UserId;
      if (userId.HasValue)
      {
        await OnUserConnectionStatusChanged(userId.Value, true);
      }
      await base.OnConnectedAsync();
    }
    catch (Exception exception)
    {
      ElmahExtensions.RaiseError(exception);
      throw;
    }
  }


  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    try
    {
      logger.LogEvent(SystemEventType.GameHubOnDisconnectedAsync, UserId, Context.ConnectionId);
      int? userId = UserId;
      if (userId.HasValue)
      {
        await OnUserConnectionStatusChanged(userId.Value, false);
      }
      await base.OnDisconnectedAsync(exception);
    }
    catch (Exception e)
    {
      ElmahExtensions.RaiseError(e);
      throw;
    }
  }
  private async Task OnUserConnectionStatusChanged(int userId, bool isConnected)
  {
    var connections = (await memoryCacheService.GetUserSignalRConnections(userId)).ToList();
    if (isConnected)
    {
      if (!connections.Contains(Context.ConnectionId))
        connections.Add(Context.ConnectionId);
    }
    else
    {
      if (connections.Contains(Context.ConnectionId))
        connections.Remove(Context.ConnectionId);
    }
    memoryCacheService.SetUserSignalRConnections(userId, connections);

    var participate = (await participateRepository.GetActiveParticipations(userId)).FirstOrDefault();

    if (participate != null)
    {
      string groupName = participate.GameId.ToString();
      if (isConnected)
      {
        TimerManagementService.OnUserTimerStop(userId);
        await notificationService.AddToGroupAsync(userId, Context.ConnectionId, groupName);
        IEnumerable<int> userIds = await memoryCacheService.GetSignalRGroupOnlineUsers(groupName);
        await Clients.Group(groupName).GameCommand(SignalCommands.GameMemberConnected, userIds);
        UnusualSuspectServiceResult<bool> done = await gameService.StartGameIfAllUsersOnline(participate.GameId, userIds);
        if (done.Success && done.Result)
        {
          await gameService.SaveChangesAsync();
          memoryCacheService.ClearGameWithDetails(participate.GameId);
        }
      }
      else
      {
        await notificationService.RemoveFromGroupAsync(userId, groupName, Context.ConnectionId);
        await Clients.Group(groupName).GameCommand(SignalCommands.GameMemberDisConnected, await memoryCacheService.GetSignalRGroupOnlineUsers(groupName));
        timerManagementService.OnUserTimerStart(userId, UserTimerEnum.OutOfGameTimeout);
      }
    }
    else
    {
      //send signal for pregame groups
      if (!isConnected)
      {
        await notificationService.RemoveFromAllGroupsAsync(userId);
      }
    }
  }
  #endregion Events

}