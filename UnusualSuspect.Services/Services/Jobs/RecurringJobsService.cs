using ElmahCore;
using Hangfire;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services.Jobs;

public class RecurringJobsService(IRankingService rankingService,
  IPreGameService preGameService,
  IGameService gameService,
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
  private static readonly SemaphoreSlim SemaphoreCheckAllUncheckedPayments = new SemaphoreSlim(1, 1);
  [DisableConcurrentExecution(timeoutInSeconds: 60)]
  public async Task CheckAllUncheckedPaymentsAsync(CancellationToken cancellationToken)
  {
    try
    {
      await SemaphoreCheckAllUncheckedPayments.WaitAsync(cancellationToken);
      await paymentUserService.CheckAllUncheckedPayments(null, cancellationToken);
    }
    catch (Exception e)
    {
      ElmahExtensions.RaiseError(e);
    }
    finally
    {
      SemaphoreCheckAllUncheckedPayments.Release();
    }
  }
  [DisableConcurrentExecution(timeoutInSeconds: 30 * 60)]
  public async Task CloseExpiredGamesAsync(CancellationToken cancellationToken)
  {
    try
    {
      await gameService.CloseExpiredGamesAsync(cancellationToken);
    }
    catch (Exception e)
    {
      ElmahExtensions.RaiseError(e);
    }

  }
}