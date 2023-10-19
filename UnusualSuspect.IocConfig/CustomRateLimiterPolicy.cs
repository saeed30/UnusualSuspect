using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace UnusualSuspect.IocConfig
{
	public class CustomRateLimiterPolicy : IRateLimiterPolicy<string>
	{
		public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } =
				(context, _) =>
				{
					context.HttpContext.Response.StatusCode = 429;
					return new ValueTask();
				};
		public RateLimitPartition<string> GetPartition(HttpContext httpContext)
		{
			if (httpContext.User.Identity?.IsAuthenticated == true)
			{
				return RateLimitPartition.GetFixedWindowLimiter(httpContext.User.Identity.Name!,
						partition => new FixedWindowRateLimiterOptions
						{
							AutoReplenishment = true,
							PermitLimit = 1_000,
							Window = TimeSpan.FromMinutes(1),
						});
			}

			return RateLimitPartition.GetFixedWindowLimiter(httpContext.Request.Headers.Host.ToString(),
					partition => new FixedWindowRateLimiterOptions
					{
						AutoReplenishment = true,
						PermitLimit = 100,
						Window = TimeSpan.FromMinutes(1),
					});
		}
	}
}
