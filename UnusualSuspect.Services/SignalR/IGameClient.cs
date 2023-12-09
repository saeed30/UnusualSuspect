using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Game;

namespace UnusualSuspect.Services.SignalR;
public interface IGameClient
{
  Task SendGameFlow(GameFlowDto gameFlowDto);
  Task SendGameBase(GameBaseDto gameBaseDto);
  Task SendGame(GameGetResponse gameBaseDto);
  Task ChangeConnectionStatus(int userId, bool isConnected);
  Task ReceiveMessage(string message);

}
