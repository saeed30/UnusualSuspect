using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.InnerModels.Game;

namespace UnusualSuspect.Services.Contracts;

public interface INotificationService
{
  Task SendGameFlowToAllMembers(GameFlowDto gameFlowDto);
  Task SendGameBaseToAllMembers(GameBaseDto gameBaseDto);
  Task SendGameModelToAllMembers(GameGetResponse game, bool addMembersToGroup = true);
  Task AddToGroupAsync(int userId, string connectionId, string groupName);
}