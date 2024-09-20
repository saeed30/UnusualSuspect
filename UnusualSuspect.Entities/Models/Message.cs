using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.Models;

public class Message : BaseEntity
{
  [MaxLength(4000)]
  public string MessageContent { get; set; }

  public int SenderUserId { get; set; }
  [ForeignKey("SenderUserId")]
  public virtual ApplicationUser SenderUser { get; set; }
  public DateTime SendTime { get; set; }
  public bool IsHidden { get; set; }
  public bool Deleted { get; set; }
  public virtual ICollection<MessageReceiver> MessageReceivers { set; get; }
}