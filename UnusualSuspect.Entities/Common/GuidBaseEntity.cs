using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.Common
{
  public abstract class GuidBaseEntity<T>: BaseEntity<T>
  {
    public Guid Guid { get; set; }
  }

  public abstract class GuidBaseEntity : GuidBaseEntity<int>
  {
  }
}
