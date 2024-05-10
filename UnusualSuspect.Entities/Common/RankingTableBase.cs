using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.Common;
public class RankingTableBase
{
  public int Id { get; set; }
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser User { get; set; }
  public int ScoreSum { get; set; }
  public int Rank { get; set; }
  public DateTime AddedDateTime { get; set; }
}
