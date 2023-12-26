using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Contracts;
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
    await Clients.All.ReceiveMessage(user, message);
  }

  public async Task StartedToTalk(int userId, short orderOfParticipation, int gameId)
  {
    await Clients.Caller.ReceiveMessage("admin", "you called StartedToTalk");
    return;
  }

  public async Task FinishedTalking(int userId, short orderOfParticipation, int gameId)
  {
    await Clients.Caller.ReceiveMessage("admin", "you called FinishedTalking");
    await turnOfPlayService.UserTurnFinishedAsync(gameId, orderOfParticipation);
  }

  public async Task CandidateCard(int userId, short? cardId, int gameId)
  {
    await Clients.Caller.ReceiveMessage("admin", "you called CandidateCard");
    await turnOfPlayService.ChangedCandidateCard(userId, cardId, gameId);
  }

  #endregion PublicMethods

  #region Events
  public override async Task OnConnectedAsync()
  {
    logger.LogWarning("User Connected to SignalR. connectionId: {ConnectionId}", Context.ConnectionId);
    int? userId = UserId;
    if (userId.HasValue)
    {
      await OnUserConnectionStatusChanged(userId.Value, true);
    }
    await base.OnConnectedAsync();
  }


  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    logger.LogWarning("User Disconnected from SignalR. connectionId: {ConnectionId}", Context.ConnectionId);
    int? userId = UserId;
    if (userId.HasValue)
    {
      await OnUserConnectionStatusChanged(userId.Value, false);
    }
    await base.OnDisconnectedAsync(exception);
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
    var participate = await participateRepository.GetActiveParticipations(userId);
    if (participate.Any())
    {
      if (isConnected)
        await notificationService.AddToGroupAsync(userId, Context.ConnectionId, participate.First().GameId.ToString());
      //await Clients.OthersInGroup(participate.Result.GameId.ToString()).ChangeConnectionStatus(userId, isConnected);
    }
  }
  #endregion Events

}