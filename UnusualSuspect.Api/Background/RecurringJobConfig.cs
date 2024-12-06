using System;
using System.Threading;
using Hangfire;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Background
{
  public static class RecurringJobConfig
  {
    public static void Config()
    {
      RecurringJob.AddOrUpdate<IRecurringJobsService>("ScoreAndRankingJob",
        job => job.CalculateScoreAndRankingsAsync(CancellationToken.None),
        Cron.Daily(1),
        new RecurringJobOptions()
        {
          TimeZone = TimeZoneInfo.Local
        });
      RecurringJob.AddOrUpdate<IRecurringJobsService>("DeleteExpiredPregameGroups",
        job => job.DeleteExpiredPregameGroupsAsync(CancellationToken.None),
        "*/30 * * * *",
        new RecurringJobOptions()
        {
          TimeZone = TimeZoneInfo.Local
        });
      RecurringJob.AddOrUpdate<IRecurringJobsService>("CheckAllUncheckedPayments",
        job => job.CheckAllUncheckedPaymentsAsync(CancellationToken.None),
        "*/2 * * * *",
        new RecurringJobOptions()
        {
          TimeZone = TimeZoneInfo.Local
        });
      RecurringJob.AddOrUpdate<IRecurringJobsService>("CloseExpiredGames",
        job => job.CloseExpiredGamesAsync(CancellationToken.None),
        "0 */2 * * *",
        new RecurringJobOptions()
        {
          TimeZone = TimeZoneInfo.Local
        });
    }
  }
}
