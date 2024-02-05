namespace UnusualSuspect.ViewModels.Settings;

public class RateLimiterSetting
{
  public int AuthenticatedUserRequestPeriodInSeconds { get; set; }
  public int AuthenticatedUserAllowedRequestCount { get; set; }
  public int AnonymousUserRequestPeriodInSeconds { get; set; }
  public int AnonymousUserAllowedRequestCount { get; set; }
}