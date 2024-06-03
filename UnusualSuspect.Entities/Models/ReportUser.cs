using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.Models
{
  public class ReportUser : BaseEntity
  {
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; }
    public int ReportedUserId { get; set; }
    [ForeignKey("ReportedUserId")]
    public virtual ApplicationUser ReportedUser { get; set; }
    [StringLength(2000)]
    public string UserDescription { get; set; }
    public DateTime DateTimeAdded { get; set; }
    public int? GameId { get; set; }
    [ForeignKey("GameId")]
    public virtual Game? Game { get; set; }
    public short ReportUserTypeId { get; set; }
    [ForeignKey("ReportUserTypeId")]
    public virtual ReportUserType ReportUserType { get; set; }

  }
}
