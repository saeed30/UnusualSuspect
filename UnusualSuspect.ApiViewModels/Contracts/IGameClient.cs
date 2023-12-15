using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.ApiViewModels.Contracts
{
    public interface IGameClient
  {
    Task GameCommand(SignalCommands command, object? data = null);
    Task ReceiveMessage(string user, string message);
  }
}
