using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class AvatarPackageUser : GuidBaseEntity
{
  public DateTime TimeAdded { get; set; }
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser ApplicationUser { get; set; }
  public short AvatarPackageId { get; set; }
  [ForeignKey("AvatarPackageId")]
  public virtual AvatarPackage AvatarPackage { get; set; }
}