using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class ScoreRepository(IUnitOfWork uow,
  ILogger<ScoreRepository> logger,
  IDapperRepository dapperRepository) : EfRepository<Score>(uow, logger), IScoreRepository
{
  public async Task RecalculateUserTotalScoreAsync(CancellationToken cancellationToken = default)
  {
    await dapperRepository.ExecuteAsync(
      "UPDATE AspNetUsers SET CalculatedScore = (SELECT ISNULL(SUM(Amount), 0) FROM Score WHERE Score.UserId = AspNetUsers.Id)",
      cancellationToken: cancellationToken);
  }

  public async Task RecalculateUserMonthScoreAsync(CancellationToken cancellationToken = default)
  {
    await dapperRepository.ExecuteAsync(
      @"UPDATE AspNetUsers SET CalculatedMonthScore = (SELECT ISNULL(SUM(Amount), 0) FROM Score WHERE Score.UserId = AspNetUsers.Id and
Score.TimeAdded between CAST(CAST(DATEADD(day, -30, GETDATE()) AS DATE) AS DATETIME) and CAST(CAST(GETDATE() AS DATE) AS DATETIME))",
      cancellationToken: cancellationToken);
  }

  public async Task RecalculateUserWeekScoreAsync(CancellationToken cancellationToken = default)
  {
    await dapperRepository.ExecuteAsync(
      @"UPDATE AspNetUsers SET CalculatedWeekScore = (SELECT ISNULL(SUM(Amount), 0) FROM Score WHERE Score.UserId = AspNetUsers.Id and
Score.TimeAdded between CAST(CAST(DATEADD(day, -7, GETDATE()) AS DATE) AS DATETIME) and CAST(CAST(GETDATE() AS DATE) AS DATETIME))",
      cancellationToken: cancellationToken);
  }

  public async Task RecalculateUserDailyScoreAsync(CancellationToken cancellationToken = default)
  {
    await dapperRepository.ExecuteAsync(
      @"UPDATE AspNetUsers SET CalculatedDailyScore = (SELECT ISNULL(SUM(Amount), 0) FROM Score WHERE Score.UserId = AspNetUsers.Id and
Score.TimeAdded between CAST(CAST(DATEADD(day, -1, GETDATE()) AS DATE) AS DATETIME) and CAST(CAST(GETDATE() AS DATE) AS DATETIME))",
      cancellationToken: cancellationToken);
  }
}