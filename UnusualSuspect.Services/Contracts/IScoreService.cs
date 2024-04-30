using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Contracts;

public interface IScoreService
{
  Task<UnusualSuspectServiceResult<bool>> SetGameFinishedScoresAsync(
    int gameId, bool won, List<Participate> participates, CancellationToken cancellationToken = default);
}