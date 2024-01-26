using System.Threading.Tasks;

namespace UnusualSuspect.ApiViewModels.Contracts
{
  public interface IGameHub
  {
    Task SendMessage(string user, string message);
    Task StartedToTalk(int gameId);
    Task FinishedTalking(int gameId);
    Task CandidateCard(short? cardId, int gameId);
  }
}
