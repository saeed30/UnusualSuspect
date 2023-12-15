using System;
using System.Collections.Generic;
using System.Text;
using UnusualSuspect.ApiViewModels.InnerModels.PreGame;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
  public class PreGameGroupGetResponse
  {
    public int PreGameGroupId { get; set; }
    public DateTime CreatedTime { get; set; }
    public short CalculatedJoinedUsers { get; set; }
    public short GameTypeId { get; set; }
    public short PreGameGroupStatusId { get; set; }
    public int? GameId { get; set; }
    public List<JoinedPreGameDto> JoinedPreGame { get; set; } = new List<JoinedPreGameDto>();

  }
}
