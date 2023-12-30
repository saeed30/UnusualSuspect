using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories
{
  public class GameCandidateRepository(IUnitOfWork uow, ILogger<GameCandidateRepository> logger)
    : EfRepository<GameCandidate>(uow, logger)
    , IGameCandidateRepository
  {
    private readonly DbSet<GameCandidate> gameCandidates = uow.Set<GameCandidate>();
    public async Task<int> ExecuteDeleteAllGameCandidatesAsync(int gameId, CancellationToken cancellationToken = default)
    {
      return await gameCandidates.Where(x => x.GameId == gameId).ExecuteDeleteAsync(cancellationToken);
    }
  }
}
