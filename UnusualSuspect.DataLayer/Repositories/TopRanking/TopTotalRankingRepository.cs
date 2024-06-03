using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories.TopRanking;

public sealed class TopTotalRankingRepository(IUnitOfWork uow,
    ILogger<TopTotalRankingRepository> logger,
    IDapperRepository dapperRepository)
  : EfRepository<TopTotalRanking>(uow, logger), ITopTotalRankingRepository
{
  public async Task<IEnumerable<RankingTableBase>> GetAllTopRanking(int maxNumber = 100, CancellationToken token = default)
  {
    return await BaseEntity.OrderBy(x => x.Rank).Take(maxNumber).ToListAsync(token);
  }

  public async Task RecalculateTopRankings(int maxNumber = 100, CancellationToken cancellationToken = default)
  {
    if (maxNumber < 1)
      maxNumber = 100;
    await dapperRepository.ExecuteAsync(@"
delete TopTotalRanking
insert into TopTotalRanking
(UserId,ScoreSum,[Rank],AddedDateTime)
select top(" + maxNumber + @") Id, CalculatedScore, Ranking, GETDATE() from AspNetUsers
order by Ranking", cancellationToken: cancellationToken);
  }
}