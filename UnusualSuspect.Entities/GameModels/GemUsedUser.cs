using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class GemUsedUser : BaseEntity
{
  public int Amount { get; set; }
  public DateTime TimeAdded { get; set; }
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser ApplicationUser { get; set; }
  public short UsedForPriceTypeId { get; set; }
  [ForeignKey("UsedForPriceTypeId")]
  public virtual PriceType UsedForPriceType { get; set; }
  public Guid ReferenceGuid { get; set; }
}