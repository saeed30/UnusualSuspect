using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.IocConfig;

public class CustomRateLimiterPolicy(IOptions<ProjectSetting> setting) : IRateLimiterPolicy<string>
{
  public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } =
    (context, _) =>
    {
      if (!context.HttpContext.Response.HasStarted)
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
      return new ValueTask();
    };
  public RateLimitPartition<string> GetPartition(HttpContext httpContext)
  {
    RateLimiterSetting rateSetting = setting.Value.RateLimiterSetting;
    if (httpContext.User.Identity?.IsAuthenticated == true)
    {
      if (GetIsLowRateEndpoint(httpContext.Request.Path.ToString().ToLower()))
      {
        return RateLimitPartition.GetFixedWindowLimiter(httpContext.User.Identity.Name!,
          _ => new FixedWindowRateLimiterOptions
          {
            AutoReplenishment = true,
            PermitLimit = rateSetting.LowRateApiAllowedRequestCount,
            Window = TimeSpan.FromSeconds(rateSetting.LowRateApiRequestPeriodInSeconds),
          });
      }
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

  private readonly string[] apiWithLowRateLimit = { "/api/log/bugreport", "/api/log/error" };
  private bool GetIsLowRateEndpoint(string endpoint)
  {
    foreach (string s in apiWithLowRateLimit)
      if (endpoint.StartsWith(s))
        return true;
    return false;
  }
}