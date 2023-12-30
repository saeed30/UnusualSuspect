using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository
{
  public interface IGameCandidateRepository : IAsyncRepository<GameCandidate>
  {
    Task<int> ExecuteDeleteAllGameCandidatesAsync(int gameId, CancellationToken cancellationToken = default);
  }
}
