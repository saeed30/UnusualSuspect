using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;

public interface ITopRankingBaseRepository
{
  Task<IEnumerable<RankingTableBase>> GetAllTopRanking(int maxNumber = 100, CancellationToken cancellationToken = default);
  Task RecalculateTopRankings(int maxNumber = 100, CancellationToken cancellationToken = default);
}