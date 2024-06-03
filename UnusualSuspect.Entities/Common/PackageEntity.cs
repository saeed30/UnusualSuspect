using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Entities.Common
{
  public class PackageEntity : BaseEnumEntity
  {
    public int Amount { get; set; }
    public int Price { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublic { get; set; }
    public string ImageUrl { get; set; }
    public short? PriceTypeId { get; set; }
    [ForeignKey("PriceTypeId")]
    public virtual PriceType? PriceType { get; set; }
    public short ViewOrder { get; set; } = 0;

  }
}
