using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class PaymentCafeBazaar : BaseEntity
{
  public string PurchaseToken { get; set; }
  public string? ValidationError { get; set; }
  public string PackageName { get; set; }
  public DateTime? ValidationCheckDateTime { get; set; }
  public bool? IsValid { get; set; }
  public int PaymentUserId { get; set; }
  [ForeignKey("PaymentUserId")]
  public PaymentUser PaymentUser { get; set; }
}