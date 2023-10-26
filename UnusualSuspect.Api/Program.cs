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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Hangfire Client
builder.Services.AddHangfire(config => config
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseSqlServerStorage(projectSetting.ConnectionStrings.HangfireConnectionString));
// Hangfire Server
builder.Services.AddHangfireServer();

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
builder.Services.AddHostedService<AlwaysRunningBackgroundService>();

/////////////////////////////////////
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseElmahCore(projectSetting);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
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
			User = "saeed",
			Pass = "S@ed1111"
		}
	}
});

app.Run();