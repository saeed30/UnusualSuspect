using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class StickerPackageUser : GuidBaseEntity
{
  public DateTime TimeAdded { get; set; }
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser ApplicationUser { get; set; }
  public short StickerPackageId { get; set; }
  [ForeignKey("StickerPackageId")]
  public virtual StickerPackage StickerPackage { get; set; }
}