using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Services;

namespace UnusualSuspect.Services.SignalR;

[Authorize]
public sealed class GameHub(IGameService gameService, INotificationService notificationService,
  IMemoryCacheService memoryCacheService) : Hub<IGameClient>
{
  private int? UserId
  {
    get
    {
      if (Context.User == null || Context.User.Identity == null ||
         !Context.User.Identity.IsAuthenticated || Context.User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
        return null;
      return Context.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt();
    }
  }
  public override async Task OnConnectedAsync()
  {
    int? userId = UserId;
    if (userId.HasValue)
    {
      await OnUserConnectionStatusChanged(userId.Value, true);
    }
    await base.OnConnectedAsync();
  }


  public override async Task OnDisconnectedAsync(Exception exception)
  {
    int? userId = UserId;
    if (userId.HasValue)
    {
      await OnUserConnectionStatusChanged(userId.Value, false);
    }
    await base.OnDisconnectedAsync(exception);
  }
  private async Task OnUserConnectionStatusChanged(int userId, bool isConnected)
  {
    var connections = memoryCacheService.GetUserSignalRConnections(userId);
    if (isConnected)
    {
      if(!connections.Contains(Context.ConnectionId))
        connections.Add(Context.ConnectionId);
    }
    else
    {
      if (connections.Contains(Context.ConnectionId))
        connections.Remove(Context.ConnectionId);
    }
    memoryCacheService.SetUserSignalRConnections(userId, connections);
    var participate = await gameService.GetActiveParticipateByUserIdAsync(userId);
    if (participate.Success && participate.Result != null)
    {
      if (isConnected)
        await notificationService.AddToGroupAsync(userId, Context.ConnectionId, participate.Result.GameId.ToString());
      await Clients.OthersInGroup(participate.Result.GameId.ToString()).ChangeConnectionStatus(userId, isConnected);
    }
  }

}