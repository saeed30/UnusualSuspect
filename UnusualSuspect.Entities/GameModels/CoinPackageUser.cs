using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class CoinPackageUser : GuidBaseEntity, IEntity<int>
{
  public int Amount { get; set; }
  public DateTime TimeAdded { get; set; }
  public int UserId { get; set; }
  [ForeignKey("UserId")]
  public virtual ApplicationUser ApplicationUser { get; set; }
  public short CoinPackageId { get; set; }
  [ForeignKey("CoinPackageId")]
  public virtual CoinPackage CoinPackage { get; set; }
}