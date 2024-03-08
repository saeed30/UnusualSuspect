
using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.Common;

public interface IEntity
{
}
public interface IEntity<TKey> : IEntity
{
    TKey Id { get; set; }
}

public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
    [NotMapped]
    public string? UserName { set; get; }
}

public abstract class BaseEntity : BaseEntity<int>
{
}
