using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Services.Contracts;
using Hangfire;
using ElmahCore;
using System;

namespace UnusualSuspect.Api.Background;

public class DailyJobs(IRankingService rankingService)
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