namespace UnusualSuspect.Services.Contracts;
public interface IDailyJobsService
{
  Task CalculateScoreAndRankingsAsync(CancellationToken cancellationToken);
}
