using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class Game : BaseEntity
{
	public DateTime CreateTime { get; set; }
	public DateTime? FinishedTime { get; set; }
	public bool? WonTheGame { get; set; }
	public short GameTypeId { get; set; }
  [ForeignKey("GameTypeId")]
	public virtual GameType GameType { get; set; }

	public virtual ICollection<CharacterCardGame> CharacterCardGames { get; set; }
	public virtual ICollection<Participate> Participates { get; set; }
	public virtual ICollection<QuestionGame> QuestionGames { get; set; }


}