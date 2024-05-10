using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services
{
  public sealed class RankingService(ITopWeekRankingRepository topWeekRatingRepository,
    ITopDayRankingRepository topDayRatingRepository,
    ITopMonthRankingRepository topMonthRatingRepository,
    ITopTotalRankingRepository topTotalRatingRepository,
    IScoreRepository scoreRepository,
    IApplicationUserRepository applicationUserRepository,
    IApplicationUserManager userManager,
    ILogger<RankingService> logger) : IRankingService
  {
    public async Task<UnusualSuspectServiceResult<TopRankingGetResponse>> GetTopRankingsAsync(
      int userId, int maxNumber = 100, CancellationToken cancellationToken = default)
    {
      ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());
      if (user == null)
        return new UnusualSuspectServiceResult<TopRankingGetResponse>(
          new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserId));
      IEnumerable<RankingTableBase> dayRanking = await topDayRatingRepository.GetAllTopRanking(maxNumber, cancellationToken);
      IEnumerable<RankingTableBase> weekRanking = await topWeekRatingRepository.GetAllTopRanking(maxNumber, cancellationToken);
      IEnumerable<RankingTableBase> monthRanking = await topMonthRatingRepository.GetAllTopRanking(maxNumber, cancellationToken);
      IEnumerable<RankingTableBase> totalRanking = await topTotalRatingRepository.GetAllTopRanking(maxNumber, cancellationToken);
      return new UnusualSuspectServiceResult<TopRankingGetResponse>(new TopRankingGetResponse()
      {
        TopDayRanking = dayRanking.ToFinishedResponse().ToList(),
        TopWeekRanking = weekRanking.ToFinishedResponse().ToList(),
        TopMonthRanking = monthRanking.ToFinishedResponse().ToList(),
        TopTotalRanking = totalRanking.ToFinishedResponse().ToList(),
        UserDayRanking = user.RankingDaily ?? -1,
        UserDayScore = user.CalculatedDailyScore,
        UserMonthRanking = user.RankingMonthly ?? -1,
        UserMonthScore = user.CalculatedMonthScore,
        UserTotalRanking = user.Ranking ?? -1,
        UserTotalScore = user.CalculatedScore,
        UserWeekRanking = user.RankingWeekly ?? -1,
        UserWeekScore = user.CalculatedWeekScore
      });
    }

    public async Task RecalculateAllRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken = default)
    {
      logger.LogWarning("RecalculateAllRankings job started at {startTime}", DateTime.Now);

      await RecalculateDailyRankings(numberOfUsersInRankingTables, cancellationToken);
      await RecalculateWeekRankings(numberOfUsersInRankingTables, cancellationToken);
      await RecalculateMonthRankings(numberOfUsersInRankingTables, cancellationToken);
      await RecalculateTotalRankings(numberOfUsersInRankingTables, cancellationToken);

      logger.LogWarning("RecalculateAllRankings job ended at {endTime}", DateTime.Now);
    }

    private async Task RecalculateTotalRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken)
    {
      logger.LogInformation("RecalculateTotalRankings started at {startTime}", DateTime.Now);
      await scoreRepository.RecalculateUserTotalScoreAsync(cancellationToken);
      await applicationUserRepository.RecalculateRaking(cancellationToken);
      await topTotalRatingRepository.RecalculateTopRankings(numberOfUsersInRankingTables, cancellationToken);
      logger.LogInformation("RecalculateTotalRankings ended at {endTime}", DateTime.Now);
    }

    private async Task RecalculateMonthRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken)
    {
      logger.LogInformation("RecalculateMonthRankings started at {startTime}", DateTime.Now);
      await scoreRepository.RecalculateUserMonthScoreAsync(cancellationToken);
      await applicationUserRepository.RecalculateMonthRaking(cancellationToken);
      await topMonthRatingRepository.RecalculateTopRankings(numberOfUsersInRankingTables, cancellationToken);
      logger.LogInformation("RecalculateMonthRankings ended at {endTime}", DateTime.Now);
    }

    private async Task RecalculateWeekRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken)
    {
      logger.LogInformation("RecalculateWeekRankings started at {startTime}", DateTime.Now);
      await scoreRepository.RecalculateUserWeekScoreAsync(cancellationToken);
      await applicationUserRepository.RecalculateWeekRaking(cancellationToken);
      await topWeekRatingRepository.RecalculateTopRankings(numberOfUsersInRankingTables, cancellationToken);
      logger.LogInformation("RecalculateWeekRankings ended at {endTime}", DateTime.Now);
    }

    private async Task RecalculateDailyRankings(int numberOfUsersInRankingTables, CancellationToken cancellationToken)
    {
      logger.LogInformation("RecalculateDailyRankings started at {startTime}", DateTime.Now);
      await scoreRepository.RecalculateUserDailyScoreAsync(cancellationToken);
      await applicationUserRepository.RecalculateDailyRaking(cancellationToken);
      await topDayRatingRepository.RecalculateTopRankings(numberOfUsersInRankingTables, cancellationToken);
      logger.LogInformation("RecalculateDailyRankings ended at {endTime}", DateTime.Now);
    }
  }
}
