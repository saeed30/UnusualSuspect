using School.DataLayer.Common;
using School.Services.Contracts.Identity;
using School.ViewModels.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace School.IocConfig;

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
        }
    }
}
