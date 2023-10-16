using ElmahCore.Mvc;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using System;

namespace UnusualSuspect.IocConfig;

public static class ElmahConfigurationExtensions
{
    public static IApplicationBuilder UseElmahCore(this IApplicationBuilder app, SiteSetting siteSettings)
    {
        app.UseWhen(context => context.Request.Path.StartsWithSegments(siteSettings.ElmahPath, StringComparison.OrdinalIgnoreCase), appBuilder =>
        {
            appBuilder.Use((ctx, next) =>
            {
                ctx.Features.Get<IHttpBodyControlFeature>().AllowSynchronousIO = true;
                return next();
            });
        });
        app.UseElmah();
        return app;
    }
}
