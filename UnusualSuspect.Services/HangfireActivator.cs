using Hangfire;

namespace UnusualSuspect.Services;

public class HangfireActivator : JobActivator
{
  private readonly IServiceProvider serviceProvider;

  public HangfireActivator(IServiceProvider serviceProvider)
  {
    this.serviceProvider = serviceProvider;
  }

  public override object ActivateJob(Type jobType)
  {
    return serviceProvider.GetService(jobType);
  }
}