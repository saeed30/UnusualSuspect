using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public sealed class GemPackage : PackageEntity, IEntity<short>
{
  public short RepetitionTypeId { get; set; }
  [ForeignKey("RepetitionTypeId")]
  public RepetitionType RepetitionType { get; set; }
}