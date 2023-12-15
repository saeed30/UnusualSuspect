using System;

namespace UnusualSuspect.ApiViewModels.InnerModels.PreGame
{
  public sealed class PreGameGroupDto
  {
    public DateTime CreatedTime { get; set; }
    public DateTime? ReadyToGameTime { get; set; }
    public short CalculatedJoinedUsers { get; set; }
    public short GameTypeId { get; set; }
    public short PreGameGroupStatusId { get; set; }
    public int? GameId { get; set; }
  }
}
