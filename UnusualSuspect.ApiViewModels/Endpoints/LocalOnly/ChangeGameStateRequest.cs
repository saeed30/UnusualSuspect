using UnusualSuspect.ApiViewModels.Enums.BaseData;

namespace UnusualSuspect.ApiViewModels.Endpoints.LocalOnly
{
  public class ChangeGameStateRequest
  {
    public int GameId { get; set; }
    public GameStatusEnum GameStatus { get; set; }
  }
}
