using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories.TopRanking;

public sealed class TopTotalRankingRepository(IUnitOfWork uow, ILogger<TopTotalRankingRepository> logger)
  : EfRepository<TopTotalRanking>(uow, logger), ITopTotalRankingRepository
{
  public async Task<IEnumerable<RankingTableBase>> GetAllTopRanking(int maxNumber = 100, CancellationToken token = default)
  {
    return await baseEntity.OrderBy(x => x.Rank).Take(maxNumber).ToListAsync(token);
  }
}