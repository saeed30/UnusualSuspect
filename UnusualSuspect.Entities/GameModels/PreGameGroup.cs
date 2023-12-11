using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class PreGameGroup : BaseEntity
{
	public DateTime CreatedTime { get; set; }
	public DateTime? ReadyToGameTime { get; set; }
	//stores number of users in this preGame to speed up the queue calculations
	public short CalculatedJoinedUsers { get; set; }
	public short GameTypeId { get; set; }
	[ForeignKey("GameTypeId")]
	public virtual GameType GameType { get; set; }
	public short PreGameGroupStatusId { get; set; }
	[ForeignKey("PreGameGroupStatusId")]
	public virtual PreGameGroupStatus PreGameGroupStatus { get; set; }
	public int? GameId { get; set; }
	[ForeignKey("GameId")]
	public virtual Game? Game { get; set; }
	public virtual ICollection<JoinedPreGame> JoinedPreGames { get; set; }

  [NotMapped]
  public string CreatedTimePersian
  {
    get
    {
      return CreatedTime.ToString();
    }
  }
  [NotMapped]
  public string ReadyToGameTimePersian
  {
    get
    {
      return ReadyToGameTime == null ? "" : ReadyToGameTime.ToString();
    }
  }

}