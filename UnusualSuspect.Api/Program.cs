using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using UnusualSuspect.DataLayer.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.IO;
using System;
using UnusualSuspect.IocConfig;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.Configuration;
using UnusualSuspect.DataLayer.Common;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using UnusualSuspect.Api.Background;
using UnusualSuspect.DataLayer.Contracts;
using Serilog;
using UnusualSuspect.Common.Middlewares;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Serilog.Events;
using System.Net.Http;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
	options.UseNamespaceRouteToken();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressInferBindingSourcesForParameters = true;
});
ConfigurationManager configuration = builder.Configuration;
builder.Services.Configure<ProjectSetting>(options => configuration.Bind(options));
ProjectSetting projectSetting = builder.Services.GetSiteSettings();
builder.Host.UseSerilog((context, loggerConfiguration) =>
	loggerConfiguration.ReadFrom.Configuration(context.Configuration));
Serilog.Debugging.SelfLog.Enable(msg =>
{
	Debug.Print(msg);
	//Debugger.Break();
});

builder.Services.AddCustomServices(configuration);

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "UnusualSuspect.Api", Version = "v1" });
	c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UnusualSuspect.Api.xml"));
	c.UseApiEndpoints();
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please insert JWT with Bearer into field",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
	 {
		 new OpenApiSecurityScheme
		 {
			 Reference = new OpenApiReference
			 {
				 Type = ReferenceType.SecurityScheme,
				 Id = "Bearer"
			 }
			},
			new string[] { }
		}
	});
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()
	.Where(a => !string.Equals(a.FullName, "Microsoft.Data.SqlClient, Version=5.0.0.0, Culture=neutral, PublicKeyToken=23ec7fc2d6eaa4a5",//for fixing error when upgrade to .Net 8
		StringComparison.OrdinalIgnoreCase)));

// Hangfire Client
builder.Services.AddHangfire(config => config
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseSqlServerStorage(projectSetting.ConnectionStrings.HangfireConnectionString));
// Hangfire Server
builder.Services.AddHangfireServer();
builder.Services.AddOutputCache();

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
builder.Services.AddHostedService<AlwaysRunningBackgroundService>();

/////////////////////////////////////

var app = builder.Build();
//avoid error for favicon request
app.Use(async (context, next) =>
{
	if (context.Request.Path.Value == "/favicon.ico")
	{
		// Favicon request, return 404
		context.Response.StatusCode = StatusCodes.Status404NotFound;
		return;
	}
	// No favicon, call next middleware
	await next.Invoke();
});
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
app.Use(async (context, next) =>
{
	context.Response.Headers.Add(
		"Content-Security-Policy",
		"font-src 'self' data:;");

	await next();
});

app.UseSerilogRequestLogging(opts =>
	{
		opts.GetLevel = (httpContext, elapsed, ex) => elapsed > 1000 ? LogEventLevel.Warning : LogEventLevel.Information;
		opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
			diagnosticContext.Set("UserName", httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated 
				? null : httpContext.User.Identity.Name);
		opts.MessageTemplate = "{RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms by {UserName}";
	}
);
app.UseHttpsRedirection();
app.UseElmahCore(projectSetting);

app.UseRouting();
app.UseRateLimiter();
app.UseOutputCache();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LogExtraInfoMiddleware>();
// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UnusualSuspect.Api V1"));

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers().RequireRateLimiting(nameof(CustomRateLimiterPolicy)).RequireAuthorization();
});

app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire", new DashboardOptions()
{
	DashboardTitle = "Hangfire dashboard",
	Authorization = new[]
	{
		new HangfireCustomBasicAuthenticationFilter()
		{
			User = projectSetting.HangfireSetting.AdminUsername,
			Pass = projectSetting.HangfireSetting.AdminPassword
		}
	}
});

app.Run();