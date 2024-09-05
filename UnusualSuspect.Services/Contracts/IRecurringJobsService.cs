namespace UnusualSuspect.Services.Contracts;
public interface IRecurringJobsService
{
  Task CalculateScoreAndRankingsAsync(CancellationToken cancellationToken);
  Task DeleteExpiredPregameGroupsAsync(CancellationToken cancellationToken);
  Task CheckAllUncheckedPayments(CancellationToken cancellationToken);

}
