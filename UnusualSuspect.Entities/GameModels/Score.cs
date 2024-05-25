using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;
public class Score : BaseEntity
{
  public int Amount { get; set; }
  public DateTime TimeAdded { get; set; }
  public int? GameId { get; set; }
  [ForeignKey("GameId")]
  public virtual Game? Game { get; set; }
  public short ScoreTypeId { get; set; }
  [ForeignKey("ScoreTypeId")]
  public virtual ScoreType ScoreType { get; set; }
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser ApplicationUser { get; set; }
}
