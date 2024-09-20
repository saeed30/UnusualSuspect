using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public sealed class CoinPackage : PackageEntity, IEntity<short>
{
  public short RepetitionTypeId { get; set; }
  [ForeignKey("RepetitionTypeId")]
  public RepetitionType RepetitionType { get; set; }
}