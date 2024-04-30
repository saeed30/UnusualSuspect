using System.Collections.Generic;

namespace UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings
{
  public class TopRankingGetResponse
  {
    public List<RankingTableBaseDto> TopDayRanking { get; set; }
    public List<RankingTableBaseDto> TopWeekRanking { get; set; }
    public List<RankingTableBaseDto> TopMonthRanking { get; set; }
    public List<RankingTableBaseDto> TopTotalRanking { get; set; }
  }
}
