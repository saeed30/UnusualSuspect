using System.Security.Claims;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Contracts;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.SignalR;

[Authorize]
public sealed class GameHub(IGameService gameService,
  IParticipateRepository participateRepository,
  INotificationService notificationService,
  IMemoryCacheService memoryCacheService,
  ILogger<GameHub> logger,
  ITurnOfPlayService turnOfPlayService) : Hub<IGameClient>, IGameHub
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
      await Clients.All.ReceiveMessage(user, message);
    }
    catch (Exception exception)
    {
      ElmahExtensions.RaiseError(exception);
      await Clients.Caller.ReceiveMessage("errorSendMessage", exception.Message);
      throw;
    }
  }

  public async Task StartedToTalk(int gameId)
  {
    try
    {
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
      await Clients.Caller.ReceiveMessage("admin", "you called FinishedTalking");
      var result = await turnOfPlayService.UserTurnFinishedAsync(UserId.Value, gameId);
      if(!result.Success)
	      await Clients.Caller.ReceiveMessage("error", result.MainError.ToString());
		}
		catch (Exception exception)
    {
      ElmahExtensions.RaiseError(exception);
      await Clients.Caller.ReceiveMessage("errorFinishedTalking", exception.Message);
      throw;
    }
  }

  public async Task CandidateCard(int? cardId, int gameId)
  {
    try
    {
	    if (!UserId.HasValue)
		    return;
      await Clients.Caller.ReceiveMessage("admin", "you called CandidateCard");
      var result = await turnOfPlayService.ChangedCandidateCard(UserId.Value, (short?)cardId, gameId);
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
      logger.LogWarning("User Connected to SignalR. connectionId: {ConnectionId}", Context.ConnectionId);
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
      logger.LogWarning("User Disconnected from SignalR. connectionId: {ConnectionId}", Context.ConnectionId);
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
    var connections = await memoryCacheService.GetUserSignalRConnections(userId);
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
        await notificationService.AddToGroupAsync(userId, Context.ConnectionId, groupName);
        List<int> userIds = await memoryCacheService.GetSignalRGroupOnlineUsers(groupName);
        await Clients.Group(groupName).GameCommand(SignalCommands.GameMemberConnected, userIds);
        await gameService.StartGameIfAllUsersOnline(participate.GameId, userIds);
        await gameService.SaveChangesAsync();
      }
      else
      {
        await notificationService.RemoveFromGroupAsync(userId, groupName, Context.ConnectionId);
        await Clients.Group(groupName).GameCommand(SignalCommands.GameMemberDisConnected, await memoryCacheService.GetSignalRGroupOnlineUsers(groupName));
      }
    }
    else if (!isConnected)
    {
      await notificationService.RemoveFromAllGroupsAsync(userId);
    }
  }
  #endregion Events

}