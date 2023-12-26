using System.Threading.Tasks;

namespace UnusualSuspect.ApiViewModels.Contracts
{
  public interface IGameHub
  {
    Task SendMessage(string user, string message);
    Task StartedToTalk(int userId, short orderOfParticipation, int gameId);
    Task FinishedTalking(int userId, short orderOfParticipation, int gameId);
    Task CandidateCard(int userId, short? cardId, int gameId);
  }
}
