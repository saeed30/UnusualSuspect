using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class Sticker : BaseEntityNotIdentity<short>
{
  public string Name { get; set; }
  public bool IsActive { get; set; }
  public bool IsFree { get; set; }
  public short? StickerPackageId { get; set; }
  [ForeignKey("StickerPackageId")]
  public virtual StickerPackage? StickerPackage { get; set; }

}