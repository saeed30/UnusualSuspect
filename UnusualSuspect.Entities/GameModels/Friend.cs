using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;
public class Friend : BaseEntity
{
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser User { get; set; }
  public int FriendUserId { get; set; }
  [ForeignKey("FriendUserId")]
  public virtual ApplicationUser FriendUser { get; set; }
  public DateTime FriendshipStartTime { get; set; }
}

