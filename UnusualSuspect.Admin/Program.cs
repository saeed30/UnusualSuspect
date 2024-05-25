using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common;
using UnusualSuspect.IocConfig;
using UnusualSuspect.ViewModels.Settings;
using System.Globalization;
using UnusualSuspect.Services.Services;
using Serilog;
using UnusualSuspect.Common.Middlewares;
using System.Diagnostics;
using Serilog.Events;
using UnusualSuspect.Services.SignalR;
using UnusualSuspect.DataLayer;

var builder = WebApplication.CreateBuilder(args);
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

builder.Services.AddCustomIdentityServices();

builder.Services.AddSiteCustomServices(configuration);
builder.Services.AddScoped<UserFilters>();

builder.Services.AddMvc(a => a.UseStringModelBinder());
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
}).AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: "AllowAll",
    b =>
    {
      b.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((host) => true);
    });
});
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
builder.Services.AddKendo();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

///////////////////////////////////////////////////////

var app = builder.Build();


if (app.Environment.IsDevelopment())
	app.UseDeveloperExceptionPage();
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
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
    RequestPath = new PathString("/wwwroot")
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LogExtraInfoMiddleware>();

//app.UseSession();
var supportedCultures = new[]
{
    new CultureInfo("fa-IR")
    {
        NumberFormat =
        {
            NumberDecimalSeparator="."
        }
    }
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("fa-IR")
    {
        NumberFormat =
                 {
                     NumberDecimalSeparator="."
                 }
    }),
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});
app.UseCors("AllowAll");
app.MapHub<GameHub>("GameHub", option =>
{
  //option.CloseOnAuthenticationExpiration = true;
});

FileService.ActivateAspose();
//Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkgpgFGkUl79uxVs8X+uspx6K+tqdtOB5G1S6PFPRrlVNvMUiSiNYl724EZbrUAWwAYHlGLRbvxMviMExTh2l9xZJ2xc4K1z3ZVudRpQpuDdFq+fe0wKXSKlB6okl0hUd2ikQHfyzsAN8fJltqvGRa5LI8BFkA/f7tffwK6jzW5xYYhHxQpU3hy4fmKo/BSg6yKAoUq3yMZTG6tWeKnWcI6ftCDxEHd30EjMISNn1LCdLN0/4YmedTjM7x+0dMiI2Qif/yI+y8gmdbostOE8S2ZjrpKsgxVv2AAZPdzHEkzYSzx81RHDzZBhKRZc5mwWAmXsWBFRQol9PdSQ8BZYLqvJ4Jzrcrext+t1ZD7HE1RZPLPAqErO9eo+7Zn9Cvu5O73+b9dxhE2sRyAv9Tl1lV2WqMezWRsO55Q3LntawkPq0HvBkd9f8uVuq9zk7VKegetCDLb0wszBAs1mjWzN+ACVHiPVKIk94/QlCkj31dWCg8YTrT5btsKcLibxog7pv1+2e4yocZKWsposmcJbgG0";

//string stimulKey = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\License.Key";
//if (System.IO.File.Exists(stimulKey))
//    StiLicense.LoadFromFile(stimulKey);

app.Run();
