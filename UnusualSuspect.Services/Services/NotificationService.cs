using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Game;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.SignalR;

namespace UnusualSuspect.Services.Services
{
  public class NotificationService(IHubContext<GameHub, IGameClient> context,
    IMemoryCacheService memoryCacheService) : INotificationService
  {
    public async Task SendGameFlowToAllMembers(GameFlowDto gameFlowDto)
    {
      await context.Clients.Group(gameFlowDto.Id.ToString()).SendGameFlow(gameFlowDto);
    }

    public async Task SendGameBaseToAllMembers(GameBaseDto gameBaseDto)
    {
      await context.Clients.Group(gameBaseDto.Id.ToString()).SendGameBase(gameBaseDto);
    }

    public async Task SendGameModelToAllMembers(GameGetResponse game, bool addMembersToGroup = true)
    {
      if (addMembersToGroup)
      {
        List<Task> tasks = new List<Task>();
        foreach (var gameParticipantDto in game.GameBaseDto.GameParticipantDto)
        {
          var connections = await memoryCacheService.GetUserSignalRConnections(gameParticipantDto.GameUserDto.Id);
          foreach (var connection in connections)
            tasks.Add(AddToGroupAsync(gameParticipantDto.GameUserDto.Id, connection, game.GameBaseDto.Id.ToString()));
        }
        await Task.WhenAll(tasks.ToArray());
      }
      await context.Clients.Group(game.GameBaseDto.Id.ToString()).SendGame(game);
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
}
