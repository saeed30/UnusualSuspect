using ElmahCore;
using Hangfire;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services.Jobs;

public class RecurringJobsService(IRankingService rankingService,
  IPreGameService preGameService,
  IPaymentUserService paymentUserService) : IRecurringJobsService
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
  [DisableConcurrentExecution(timeoutInSeconds: 60)]
  public async Task DeleteExpiredPregameGroupsAsync(CancellationToken cancellationToken)
  {
    try
    {
      await preGameService.DeleteExpiredPregameGroupsAsync(cancellationToken);
    }
    catch (Exception e)
    {
      ElmahExtensions.RaiseError(e);
    }
  }
  [DisableConcurrentExecution(timeoutInSeconds: 60)]
  public async Task CheckAllUncheckedPayments(CancellationToken cancellationToken)
  {
    return;
    try
    {
      await paymentUserService.CheckAllUncheckedPayments(null, cancellationToken);
    }
    catch (Exception e)
    {
      ElmahExtensions.RaiseError(e);
    }
  }
}