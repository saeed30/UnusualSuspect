using System;
using System.Collections.Generic;
using System.Text;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
  public class GetPreGameGroupDetailResponse
  {
    public int PreGameGroupId { get; set; }
    public DateTime CreatedTime { get; set; }
    public short CalculatedJoinedUsers { get; set; }
    public short GameTypeId { get; set; }
    public short PreGameGroupStatusId { get; set; }
    public int? GameId { get; set; }

  }
}
