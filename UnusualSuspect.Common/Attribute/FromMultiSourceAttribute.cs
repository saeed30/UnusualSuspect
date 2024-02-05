using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UnusualSuspect.Common.Attribute;

public class FromMultiSourceAttribute : System.Attribute, IBindingSourceMetadata
{
  public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
    new[] { BindingSource.Path, BindingSource.Query },
    nameof(FromMultiSourceAttribute));
}