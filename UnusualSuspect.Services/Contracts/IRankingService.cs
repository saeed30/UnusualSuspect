using UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings;

namespace UnusualSuspect.Services.Contracts
{
  public interface IRankingService
  {
    Task<UnusualSuspectServiceResult<TopRankingGetResponse>> GetTopRankingsAsync(int maxNumber = 100, CancellationToken cancellationToken = default);
    Task RecalculateAllRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken = default);
  }
}
