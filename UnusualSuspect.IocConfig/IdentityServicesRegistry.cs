using School.IocConfig;
using Microsoft.Extensions.DependencyInjection;

namespace School.IocConfig;

public static class IdentityServicesRegistry
{
    public static void AddCustomIdentityServices(this IServiceCollection services)
    {
        var siteSettings = services.GetSiteSettings();
        services.AddIdentityOptions(siteSettings);
        services.AddConfiguredDbContext(siteSettings);
    }
}
