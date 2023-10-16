using UnusualSuspect.Common;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace UnusualSuspect.DataLayer.Common;

public static class SqlServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredMsSqlDbContext(this IServiceCollection services, ProjectSetting siteSettings)
    {
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        services.AddEntityFrameworkSqlServer();
        services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder) => optionsBuilder.UseConfiguredMsSql(siteSettings, serviceProvider));
        return services;
    }

    public static void UseConfiguredMsSql(
        this DbContextOptionsBuilder optionsBuilder, ProjectSetting siteSettings, IServiceProvider serviceProvider)
    {
        optionsBuilder.//UseLazyLoadingProxies().
            UseSqlServer(
                    siteSettings.ConnectionStrings.ApplicationConnectionString,
                    sqlServerOptionsBuilder =>
                    {
                        sqlServerOptionsBuilder.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
                        sqlServerOptionsBuilder.EnableRetryOnFailure();
                        sqlServerOptionsBuilder.MigrationsAssembly("UnusualSuspect.DataLayer");
                    });
        optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor());
        optionsBuilder.ConfigureWarnings(warnings =>
        {
        });
    }
    public static void UseConfiguredMsSqlWhitConnectionString(
   this DbContextOptionsBuilder optionsBuilder, string ConnectionString, IServiceProvider serviceProvider)
    {
        optionsBuilder.UseSqlServer(
                    ConnectionString,
                    sqlServerOptionsBuilder =>
                    {
                        sqlServerOptionsBuilder.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
                        sqlServerOptionsBuilder.EnableRetryOnFailure();
                        sqlServerOptionsBuilder.MigrationsAssembly("UnusualSuspect.DataLayer");
                    });
        optionsBuilder.UseInternalServiceProvider(serviceProvider); 
        optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor());
        optionsBuilder.ConfigureWarnings(warnings =>
        {
        });
    }
}
