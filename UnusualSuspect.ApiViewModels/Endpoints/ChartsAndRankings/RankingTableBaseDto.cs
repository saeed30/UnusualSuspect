using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings
{
  public class RankingTableBaseDto
  {
    public int Id { get; set; }
    public UserDto UserDto { get; set; }
    public int ScoreSum { get; set; }
    public int Rank { get; set; }
  }
}
