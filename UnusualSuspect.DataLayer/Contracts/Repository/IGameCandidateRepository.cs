using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository
{
  public interface IGameCandidateRepository : IAsyncRepository<GameCandidate>
  {
    Task<int> ExecuteDeleteAllGameCandidatesAsync(int gameId, CancellationToken cancellationToken = default);
    Task<List<GameCandidate>> GetAllGameCandidatesAsync(int gameId, CancellationToken cancellationToken = default);
    Task DeleteUserCandidatesInGameAsync(int gameId, int userId, CancellationToken cancellationToken = default);
    Task ChangeUserCandidateInGameAsync(int gameId, int userId, short characterCardId, CancellationToken cancellationToken = default);
  }
}
