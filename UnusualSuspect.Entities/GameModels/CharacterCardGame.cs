using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class CharacterCardGame : BaseEntity
{
	public bool IsMurderer { get; set; }
	public short? RemovedTurn { get; set; }
	public short CharacterCardId { get; set; }
	[ForeignKey("CharacterCardId")]
	public virtual CharacterCard CharacterCard { get; set; }
	public int GameId { get; set; }
	[ForeignKey("GameId")]
	public virtual Game Game { get; set; }

}