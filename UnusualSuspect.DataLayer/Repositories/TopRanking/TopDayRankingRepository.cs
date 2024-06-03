using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories.TopRanking;

public sealed class TopDayRankingRepository(IUnitOfWork uow, ILogger<TopDayRankingRepository> logger,
    IDapperRepository dapperRepository)
  : EfRepository<TopDayRanking>(uow, logger), ITopDayRankingRepository
{
  public async Task<IEnumerable<RankingTableBase>> GetAllTopRanking(int maxNumber = 100, CancellationToken token = default)
  {
    return await BaseEntity.Include(x => x.User).OrderBy(x => x.Rank).Take(maxNumber).ToListAsync(token);
  }

  public async Task RecalculateTopRankings(int maxNumber = 100, CancellationToken cancellationToken = default)
  {
    if (maxNumber < 1)
      maxNumber = 100;
    await dapperRepository.ExecuteAsync(@"
delete TopDayRanking
insert into TopDayRanking
(UserId,ScoreSum,[Rank],AddedDateTime)
select top(" + maxNumber + @") Id, CalculatedDailyScore, RankingDaily, GETDATE() from AspNetUsers
order by RankingDaily, Ranking", cancellationToken: cancellationToken);
  }
}