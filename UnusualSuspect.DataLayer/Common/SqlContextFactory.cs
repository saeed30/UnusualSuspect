using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace UnusualSuspect.DataLayer.Common;

public class SqlContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var services = new ServiceCollection();
        services.AddOptions();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<ILoggerFactory, LoggerFactory>();

        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
                            .SetBasePath(basePath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();
        services.AddSingleton<IConfigurationRoot>(provider => configuration);
        services.Configure<ProjectSetting>(options => configuration.Bind(options));
        var siteSettings = services.BuildServiceProvider().GetRequiredService<IOptionsSnapshot<ProjectSetting>>();
        services.AddEntityFrameworkSqlServer(); 
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseConfiguredMsSql(siteSettings.Value, services.BuildServiceProvider());
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
