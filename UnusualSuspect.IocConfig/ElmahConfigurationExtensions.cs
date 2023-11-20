using ElmahCore.Mvc;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using System.Text.Json;
using ElmahCore;
using UnusualSuspect.Common.Models;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.IocConfig;

public static class ElmahConfigurationExtensions
{
	public static IApplicationBuilder UseElmahCore(this IApplicationBuilder app, ProjectSetting projectSetting)
	{
		app.UseWhen(context => context.Request.Path.StartsWithSegments(projectSetting.SiteSetting.ElmahPath, StringComparison.OrdinalIgnoreCase), appBuilder =>
		{
			appBuilder.Use((ctx, next) =>
					{
						ctx.Features.Get<IHttpBodyControlFeature>().AllowSynchronousIO = true;
						return next();
					});
		});
		app.UseElmah();
		if (!projectSetting.IsTesting)
		{
			app.UseExceptionHandler(errorApp =>
		{
			errorApp.Run(async context =>
			{
				var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
				var exception = errorFeature.Error;
				context.Response.ContentType = "application/problem+json";
				context.Response.StatusCode = 500;
				var stream = context.Response.Body;
				var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
				var problemDetails = new ApiResult(false,
					ApiResultStatusCode.ServerError, $"appName:error: {traceId}");
				await JsonSerializer.SerializeAsync(stream, problemDetails);

				//log exception using ElmahCore
				ElmahExtensions.RaiseError(exception);
			});
		});
		}
		return app;
	}
}
