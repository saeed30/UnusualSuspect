using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.Models
{
  public class MessageReceiver : BaseEntity
  {
    public int MessageId { get; set; }
    [ForeignKey("MessageId")]
    public virtual Message Message { get; set; }
    public int ReceiverUserId { get; set; }
    [ForeignKey("ReceiverUserId")]
    public virtual ApplicationUser ReceiverUser { get; set; }
    public DateTime? ViewTime { get; set; }
    public bool IsViewed { get; set; }
    public bool Deleted { get; set; }

  }
}
