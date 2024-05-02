using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories.TopRanking;

public sealed class TopWeekRankingRepository(IUnitOfWork uow,
    ILogger<TopWeekRankingRepository> logger,
    IDapperRepository dapperRepository)
  : EfRepository<TopWeekRanking>(uow, logger), ITopWeekRankingRepository
{
  public async Task<IEnumerable<RankingTableBase>> GetAllTopRanking(int maxNumber = 100, CancellationToken token = default)
  {
    return await baseEntity.OrderBy(x => x.Rank).Take(maxNumber).ToListAsync(token);
  }

  public async Task RecalculateTopRankings(int maxNumber = 100, CancellationToken cancellationToken = default)
  {
    if (maxNumber < 1)
      maxNumber = 100;
    await dapperRepository.ExecuteAsync(@"
delete TopWeekRanking
insert into TopWeekRanking
(UserId,ScoreSum,[Rank],AddedDateTime)
select top(" + maxNumber + @") Id, CalculatedWeekScore, RankingWeekly, GETDATE() from AspNetUsers
order by RankingWeekly, Ranking", cancellationToken: cancellationToken);
  }
}