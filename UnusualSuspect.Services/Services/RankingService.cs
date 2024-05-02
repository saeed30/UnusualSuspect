using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services
{
  public sealed class RankingService(ITopWeekRankingRepository topWeekRatingRepository,
    ITopDayRankingRepository topDayRatingRepository,
    ITopMonthRankingRepository topMonthRatingRepository,
    ITopTotalRankingRepository topTotalRatingRepository,
    IScoreRepository scoreRepository,
    IApplicationUserRepository applicationUserRepository,
    ILogger<RankingService> logger) : IRankingService
  {
    public async Task<UnusualSuspectServiceResult<TopRankingGetResponse>> GetTopRankingsAsync(
      int maxNumber = 100, CancellationToken cancellationToken = default)
    {
      IEnumerable<RankingTableBase> dayRanking = await topDayRatingRepository.GetAllTopRanking(maxNumber, cancellationToken);
      IEnumerable<RankingTableBase> weekRanking = await topWeekRatingRepository.GetAllTopRanking(maxNumber, cancellationToken);
      IEnumerable<RankingTableBase> monthRanking = await topMonthRatingRepository.GetAllTopRanking(maxNumber, cancellationToken);
      IEnumerable<RankingTableBase> totalRanking = await topTotalRatingRepository.GetAllTopRanking(maxNumber, cancellationToken);
      return new UnusualSuspectServiceResult<TopRankingGetResponse>(new TopRankingGetResponse()
      {
        TopDayRanking = dayRanking.ToFinishedResponse().ToList(),
        TopWeekRanking = weekRanking.ToFinishedResponse().ToList(),
        TopMonthRanking = monthRanking.ToFinishedResponse().ToList(),
        TopTotalRanking = totalRanking.ToFinishedResponse().ToList()
      });
    }

    public async Task RecalculateAllRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken = default)
    {
      logger.LogCritical("RecalculateAllRankings job started at {startTime}", DateTime.Now);

      await RecalculateDailyRankings(numberOfUsersInRankingTables, cancellationToken);
      await RecalculateWeekRankings(numberOfUsersInRankingTables, cancellationToken);
      await RecalculateMonthRankings(numberOfUsersInRankingTables, cancellationToken);
      await RecalculateTotalRankings(numberOfUsersInRankingTables, cancellationToken);

      logger.LogCritical("RecalculateAllRankings job ended at {endTime}", DateTime.Now);
    }

    private async Task RecalculateTotalRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken)
    {
      logger.LogCritical("RecalculateTotalRankings started at {startTime}", DateTime.Now);
      await scoreRepository.RecalculateUserTotalScoreAsync(cancellationToken);
      await applicationUserRepository.RecalculateRaking(cancellationToken);
      await topTotalRatingRepository.RecalculateTopRankings(numberOfUsersInRankingTables, cancellationToken);
      logger.LogInformation("RecalculateTotalRankings ended at {endTime}", DateTime.Now);
    }

    private async Task RecalculateMonthRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken)
    {
      logger.LogCritical("RecalculateMonthRankings started at {startTime}", DateTime.Now);
      await scoreRepository.RecalculateUserMonthScoreAsync(cancellationToken);
      await applicationUserRepository.RecalculateMonthRaking(cancellationToken);
      await topMonthRatingRepository.RecalculateTopRankings(numberOfUsersInRankingTables, cancellationToken);
      logger.LogInformation("RecalculateMonthRankings ended at {endTime}", DateTime.Now);
    }

    private async Task RecalculateWeekRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken)
    {
      logger.LogCritical("RecalculateWeekRankings started at {startTime}", DateTime.Now);
      await scoreRepository.RecalculateUserWeekScoreAsync(cancellationToken);
      await applicationUserRepository.RecalculateWeekRaking(cancellationToken);
      await topWeekRatingRepository.RecalculateTopRankings(numberOfUsersInRankingTables, cancellationToken);
      logger.LogInformation("RecalculateWeekRankings ended at {endTime}", DateTime.Now);
    }

    private async Task RecalculateDailyRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken)
    {
      logger.LogCritical("RecalculateDailyRankings started at {startTime}", DateTime.Now);
      await scoreRepository.RecalculateUserDailyScoreAsync(cancellationToken);
      await applicationUserRepository.RecalculateDailyRaking(cancellationToken);
      await topDayRatingRepository.RecalculateTopRankings(numberOfUsersInRankingTables, cancellationToken);
      logger.LogInformation("RecalculateDailyRankings ended at {endTime}", DateTime.Now);
    }
  }
}
