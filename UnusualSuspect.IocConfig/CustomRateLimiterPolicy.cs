using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.IocConfig
{
	public class CustomRateLimiterPolicy : IRateLimiterPolicy<string>
	{
		private readonly IOptions<ProjectSetting> setting;
		public CustomRateLimiterPolicy(IOptions<ProjectSetting> setting)
		{
			this.setting = setting;
		}
		public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } =
				(context, _) =>
				{
					context.HttpContext.Response.StatusCode = 429;
					return new ValueTask();
				};
		public RateLimitPartition<string> GetPartition(HttpContext httpContext)
		{
			RateLimiterSetting rateSetting = setting.Value.RateLimiterSetting;
			if (httpContext.User.Identity?.IsAuthenticated == true)
			{
				return RateLimitPartition.GetFixedWindowLimiter(httpContext.User.Identity.Name!,
						_ => new FixedWindowRateLimiterOptions
						{
							AutoReplenishment = true,
							PermitLimit = rateSetting.AuthenticatedUserAllowedRequestCount,
							Window = TimeSpan.FromSeconds(rateSetting.AuthenticatedUserRequestPeriodInSeconds),
						});
			}

			return RateLimitPartition.GetFixedWindowLimiter(httpContext.Request.Headers.Host.ToString(),
					_ => new FixedWindowRateLimiterOptions
					{
						AutoReplenishment = true,
						PermitLimit = rateSetting.AnonymousUserAllowedRequestCount,
						Window = TimeSpan.FromSeconds(rateSetting.AnonymousUserRequestPeriodInSeconds),
					});
		}
	}
}
