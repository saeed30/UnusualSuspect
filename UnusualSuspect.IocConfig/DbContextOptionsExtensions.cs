using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace UnusualSuspect.IocConfig;

public static class DbContextOptionsExtensions
{
    public static IServiceCollection AddConfiguredDbContext(
        this IServiceCollection serviceCollection, ProjectSetting siteSettings)
    {
        serviceCollection.AddConfiguredMsSqlDbContext(siteSettings);
        return serviceCollection;
    }

    /// <summary>
    /// Creates and seeds the database.
    /// </summary>
    public static void InitializeDb(this IServiceProvider serviceProvider)
    {
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var identityDbInitialize = scope.ServiceProvider.GetRequiredService<IIdentityDbInitializer>();
            identityDbInitialize.Initialize();
            identityDbInitialize.SeedData();
            //using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
            //    context.Database.Migrate();
        }
    }
}
