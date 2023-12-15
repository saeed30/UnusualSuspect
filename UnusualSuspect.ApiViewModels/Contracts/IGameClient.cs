using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.InnerModels.Game;

namespace UnusualSuspect.ApiViewModels.Contracts
{
    public interface IGameClient
  {
    Task SendGameFlow(GameFlowDto gameFlowDto);
    Task SendGameBase(GameBaseDto gameBaseDto);
    Task SendGame(GameGetResponse gameBaseDto);
    Task ChangeConnectionStatus(int userId, bool isConnected);
    Task ReceiveMessage(string user, string message);
  }
}
