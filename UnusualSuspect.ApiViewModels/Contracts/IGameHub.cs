using System.Threading.Tasks;

namespace UnusualSuspect.ApiViewModels.Contracts
{
  public interface IGameHub
  {
    Task SendMessage(string user, string message);
  }
}
