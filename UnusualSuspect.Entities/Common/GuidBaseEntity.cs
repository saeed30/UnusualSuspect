using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.Common
{
  public class GuidBaseEntity<T>
  {
    public T Id { get; set; }
    public Guid Guid { get; set; }
  }

  [NotMapped]
  public class GuidBaseEntity : GuidBaseEntity<int>
  {
  }
}
