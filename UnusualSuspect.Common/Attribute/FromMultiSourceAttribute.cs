using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Common.Attribute
{
	public class FromMultiSourceAttribute : System.Attribute, IBindingSourceMetadata
	{
		public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
				new[] { BindingSource.Path, BindingSource.Query },
				nameof(FromMultiSourceAttribute));
	}
}
