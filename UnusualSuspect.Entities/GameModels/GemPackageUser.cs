using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels
{
  public class GemPackageUser : BaseEntity
  {
    public int Amount { get; set; }
    public DateTime TimeAdded { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser ApplicationUser { get; set; }
    public short GemPackageId { get; set; }
    [ForeignKey("GemPackageId")]
    public virtual GemPackage GemPackage { get; set; }

    public string PurchaseToken { get; set; }

  }
}
