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
      RecurringJob.AddOrUpdate<IDailyJobsService>("ScoreAndRankingJob",
        job => job.CalculateScoreAndRankingsAsync(CancellationToken.None),
        Cron.Daily(1),
        new RecurringJobOptions()
        {
          TimeZone = TimeZoneInfo.Local
        });
      RecurringJob.AddOrUpdate<IFiveMinuteJobsService>("DeleteExpiredPregameGroups",
        job => job.DeleteExpiredPregameGroupsAsync(CancellationToken.None),
        "*/5 * * * *",
        new RecurringJobOptions()
        {
          TimeZone = TimeZoneInfo.Local
        });
    }
  }
}
