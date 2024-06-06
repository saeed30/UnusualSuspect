using ElmahCore;
using Hangfire;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services.Jobs;

public class DailyJobsService(IRankingService rankingService): IDailyJobsService
{
  [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
  public async Task CalculateScoreAndRankingsAsync(CancellationToken cancellationToken)
  {
    try
    {
      await rankingService.RecalculateAllRankings(100, cancellationToken);
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
    }
  }
}