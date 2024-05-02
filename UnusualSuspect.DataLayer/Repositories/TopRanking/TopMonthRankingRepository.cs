using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories.TopRanking;

public sealed class TopMonthRankingRepository(IUnitOfWork uow,
    ILogger<TopMonthRankingRepository> logger,
    IDapperRepository dapperRepository)
  : EfRepository<TopMonthRanking>(uow, logger), ITopMonthRankingRepository
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
delete TopMonthRanking
insert into TopMonthRanking
(UserId,ScoreSum,[Rank],AddedDateTime)
select top(" + maxNumber + @") Id, CalculatedMonthScore, RankingMonthly, GETDATE() from AspNetUsers
order by RankingMonthly, Ranking", cancellationToken: cancellationToken);
  }
}