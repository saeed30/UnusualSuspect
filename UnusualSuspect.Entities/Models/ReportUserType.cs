using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Models;

public sealed class ReportUserType : BaseEnumEntity, IEntity<short>
{
  public bool IsActive { get; set; }
}