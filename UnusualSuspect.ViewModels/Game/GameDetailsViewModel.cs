using UnusualSuspect.ApiViewModels.Endpoints.Game;

namespace UnusualSuspect.ViewModels.Game;

public class GameDetailsViewModel
{
  public GameGetResponse GameGetResponse { get; set; }
  //extra info that is not in GameGetResponse
  public DateTime CreateTime { get; set; }
  public DateTime? FinishedTime { get; set; }
  public string GameStatusTitle { get; set; }

}