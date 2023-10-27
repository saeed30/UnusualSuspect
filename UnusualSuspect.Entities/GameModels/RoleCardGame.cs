using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.GameModels;

public class RoleCardGame : BaseEntity
{
	public bool IsMurderer { get; set; }
	public short? RemovedTurn { get; set; }
	public int RoleCardId { get; set; }
	[ForeignKey("RoleCardId")]
	public virtual RoleCard RoleCard { get; set; }
	public int GameId { get; set; }
	[ForeignKey("GameId")]
	public virtual Game Game { get; set; }

}