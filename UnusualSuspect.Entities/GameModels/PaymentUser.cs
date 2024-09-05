using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class PaymentUser : BaseEntity
{
  public int Amount { get; set; }
  public DateTime TimeAdded { get; set; }
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser ApplicationUser { get; set; }
  public string PurchaseToken { get; set; }
  public bool? IsValid { get; set; }
  public DateTime? ValidationCheckDateTime { get; set; }
  public short UsedForPriceTypeId { get; set; }
  [ForeignKey("UsedForPriceTypeId")]
  public virtual PriceType UsedForPriceType { get; set; }
  public Guid ReferenceGuid { get; set; }
  public short? StoreId { get; set; } = 1;
  [ForeignKey("StoreId")]
  public virtual Store? Store { get; set; }
  public string? ValidationError { get; set; }

}