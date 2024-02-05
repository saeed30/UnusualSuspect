using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class GameCandidate : BaseEntity
{
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser ApplicationUser { get; set; }
  public short CharacterCardId { get; set; }
  [ForeignKey("CharacterCardId")]
  public virtual CharacterCard CharacterCard { get; set; }
  public int GameId { get; set; }
  [ForeignKey("GameId")]
  public virtual Game Game { get; set; }

  public DateTime DateTimeAdded { get; set; }

}