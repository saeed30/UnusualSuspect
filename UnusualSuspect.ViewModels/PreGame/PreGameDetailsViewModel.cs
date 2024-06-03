using UnusualSuspect.ApiViewModels.Endpoints.PreGame;

namespace UnusualSuspect.ViewModels.PreGame
{
  public class PreGameDetailsViewModel
  {
    public PreGameGroupGetResponse PreGameGroupGetResponse { get; set; }
    public string GameTypeTitle {
      get;
      set;
    }

    public string PreGameGroupStatusTitle { get; set; }
  }
}
