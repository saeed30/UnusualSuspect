using UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Services.Mapping
{
  public static class RankingTableMapper
  {
    public static IEnumerable<RankingTableBaseDto> ToFinishedResponse(this IEnumerable<RankingTableBase> model)
    {
      return model.Select(x => x.ToFinishedResponse()).ToList();

    }
    public static RankingTableBaseDto ToFinishedResponse(this RankingTableBase model)
    {
      return new RankingTableBaseDto()
      {
        Id = model.Id,
        Rank = model.Rank,
        ScoreSum = model.ScoreSum,
        UserDto = model.ApplicationUser.ToUserDto()
      };
    }

  }
}
