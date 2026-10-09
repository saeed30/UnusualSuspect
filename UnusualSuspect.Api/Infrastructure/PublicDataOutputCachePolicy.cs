using Microsoft.AspNetCore.OutputCaching;
using System.Threading;
using System.Threading.Tasks;

namespace UnusualSuspect.Api.Infrastructure;
public sealed class PublicDataOutputCachePolicy : IOutputCachePolicy
{
	public ValueTask CacheRequestAsync(
			OutputCacheContext context,
			CancellationToken cancellationToken)
	{
		context.EnableOutputCaching = true;
		context.AllowCacheLookup = true;
		context.AllowCacheStorage = true;
		context.AllowLocking = true;

		return ValueTask.CompletedTask;
	}

	public ValueTask ServeFromCacheAsync(
			OutputCacheContext context,
			CancellationToken cancellationToken)
	{
		return ValueTask.CompletedTask;
	}

	public ValueTask ServeResponseAsync(
			OutputCacheContext context,
			CancellationToken cancellationToken)
	{
		return ValueTask.CompletedTask;
	}
}