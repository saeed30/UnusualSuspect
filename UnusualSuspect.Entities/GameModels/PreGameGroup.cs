using System.ComponentModel.DataAnnotations.Schema;

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
}