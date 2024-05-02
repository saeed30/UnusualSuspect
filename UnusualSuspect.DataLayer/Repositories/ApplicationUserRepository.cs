using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class ApplicationUserRepository(IUnitOfWork uow,
    ILogger<ApplicationUserRepository> logger,
    IDapperRepository dapperRepository)
  : EfRepository<ApplicationUser>(uow, logger), IApplicationUserRepository
{
  public async Task RecalculateRaking(CancellationToken cancellationToken = default)
  {
    await dapperRepository.ExecuteAsync(@"
WITH Ranking AS (
SELECT Id,
RANK() OVER (ORDER BY CalculatedScore DESC) AS NewRank
FROM AspNetUsers
)
UPDATE AspNetUsers
SET Ranking = Ranking.NewRank
FROM AspNetUsers
JOIN Ranking ON AspNetUsers.Id = Ranking.Id",
      cancellationToken: cancellationToken);
  }

  public async Task RecalculateMonthRaking(CancellationToken cancellationToken = default)
  {
    await dapperRepository.ExecuteAsync(@"
WITH Ranking AS (
SELECT Id,
RANK() OVER (ORDER BY CalculatedMonthScore DESC) AS NewRank
FROM AspNetUsers
)
UPDATE AspNetUsers
SET RankingMonthly = Ranking.NewRank
FROM AspNetUsers
JOIN Ranking ON AspNetUsers.Id = Ranking.Id",
      cancellationToken: cancellationToken);
  }

  public async Task RecalculateWeekRaking(CancellationToken cancellationToken = default)
  {
    await dapperRepository.ExecuteAsync(@"
WITH Ranking AS (
SELECT Id,
RANK() OVER (ORDER BY CalculatedWeekScore DESC) AS NewRank
FROM AspNetUsers
)
UPDATE AspNetUsers
SET RankingWeekly = Ranking.NewRank
FROM AspNetUsers
JOIN Ranking ON AspNetUsers.Id = Ranking.Id",
      cancellationToken: cancellationToken);
  }

  public async Task RecalculateDailyRaking(CancellationToken cancellationToken = default)
  {
    await dapperRepository.ExecuteAsync(@"
WITH Ranking AS (
SELECT Id,
RANK() OVER (ORDER BY CalculatedDailyScore DESC) AS NewRank
FROM AspNetUsers
)
UPDATE AspNetUsers
SET RankingDaily = Ranking.NewRank
FROM AspNetUsers
JOIN Ranking ON AspNetUsers.Id = Ranking.Id",
      cancellationToken: cancellationToken);
  }
}