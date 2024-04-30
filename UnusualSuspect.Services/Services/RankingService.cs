using UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services
{
  public sealed class RankingService(ITopWeekRankingRepository topWeekRatingRepository,
    ITopDayRankingRepository topDayRatingRepository,
    ITopMonthRankingRepository topMonthRatingRepository,
    ITopTotalRankingRepository topTotalRatingRepository) : IRankingService
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
  }
}
