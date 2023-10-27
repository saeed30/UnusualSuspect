using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.GameModels;

public class Game : BaseEntity
{
	public DateTime CreateTime { get; set; }
	public DateTime? FinishedTime { get; set; }
	public short GameTypeId { get; set; }
	[ForeignKey("GameTypeId")]
	public virtual GameType GameType { get; set; }

}