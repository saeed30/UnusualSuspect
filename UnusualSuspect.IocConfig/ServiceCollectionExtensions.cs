using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Exceptions;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.DataLayer.Repositories;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Identity;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.Services.JcoSecurity;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.IocConfig;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
	{
		var Settings = GetSiteSettings(services);
		services.AddConfiguredDbContext(Settings);

		services.AddIdentity<ApplicationUser, Role>(identityOptions =>
		{
			setPasswordOptions(identityOptions.Password, Settings);
		})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
		services.AddElmahCore(configuration, Settings);
		AddJwtAuthentication(services, Settings.JwtSettings);
		AddQuartzHostedService(services, Settings);
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddScoped<IPrincipal>(provider => provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current);
		services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
		services.AddScoped<IDocumentService, DocumentService>();
		services.AddScoped<ILogService, LogService>();
		services.AddScoped<IUploadServise, UploadServise>();
		services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
		services.AddScoped<IApplicationUserService, ApplicationUserService>();
		services.AddScoped<IApplicationSignInService, ApplicationSignInManager>();
		services.AddScoped<IApplicationRoleService, ApplicationRoleManager>();
		services.AddScoped<IJwtService, JwtService>();
		services.AddScoped<ISmsService, SmsService>();
		services.AddScoped<IFileService, FileService>();
		services.AddScoped<IFireBaseService, FireBaseService>();
		services.AddScoped<IJoinedPreGameRepository, JoinedPreGameRepository>();
		services.AddScoped<IGameRepository, GameRepository>();
		services.AddScoped<IParticipateRepository, ParticipateRepository>();
		services.AddScoped<IPreGameGroupRepository, PreGameGroupRepository>();
		services.AddScoped<IGameTypeRepository, GameTypeRepository>();
		services.AddScoped<ISmsLogRepository, SmsLogRepository>();
		services.AddScoped<IPreGameService, PreGameService>();
		services.AddScoped<IGameService, GameService>();


		//services.AddRateLimiter(options =>
		//{
		//	options.RejectionStatusCode = 429;
		//	options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
		//			RateLimitPartition.GetSlidingWindowLimiter(
		//					partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
		//					factory: partition => new SlidingWindowRateLimiterOptions
		//					{
		//						AutoReplenishment = true,
		//						PermitLimit = 100,
		//						QueueLimit = 0,
		//						Window = TimeSpan.FromMinutes(1),
		//						SegmentsPerWindow = 6
		//					}));
		//});
		services.AddRateLimiter(options =>
		{
			options.RejectionStatusCode = 429;
			options.AddPolicy<string, CustomRateLimiterPolicy>(nameof(CustomRateLimiterPolicy));
		});
		var provider = services.BuildServiceProvider();
		provider.InitializeDb();

		return services;

	}

	private static void AddQuartzHostedService(IServiceCollection services, ProjectSetting settings)
	{
		services.AddQuartz(q =>
		{
			q.UseDefaultThreadPool(maxConcurrency: 1);
		});
		services.AddQuartzHostedService(
				q => q.WaitForJobsToComplete = true);
	}

	public static IServiceCollection AddSiteCustomServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMvc(option =>
		{
			option.ModelBinderProviders.Insert(0, new Services.Config.DateTimeModelBinderProvider());
		});
		services.Configure<RequestLocalizationOptions>(options =>
		{
			var supportedCultures = new List<CultureInfo>
						{
											new CultureInfo("fa-IR") {
													NumberFormat ={
															NumberDecimalSeparator="."
													}
											}
						};
			options.DefaultRequestCulture = new RequestCulture(new CultureInfo("fa-IR")
			{
				NumberFormat =
						 {
										 NumberDecimalSeparator="."
						 }
			});
			options.SupportedCultures = supportedCultures;
			options.SupportedUICultures = supportedCultures;
		});
		var Settings = GetSiteSettings(services);
		services.AddElmahCore(configuration, Settings);
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddScoped<IPrincipal>(provider => provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current);
		services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
		services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
		services.AddScoped<IApplicationSignInService, ApplicationSignInManager>();
		services.AddScoped<IApplicationRoleService, ApplicationRoleManager>();
		services.AddScoped<ISmsService, SmsService>();
		services.AddScoped<IFireBaseService, FireBaseService>();
		services.AddScoped<IApplicationUserService, ApplicationUserService>();
		services.AddScoped<IDocumentService, DocumentService>();
		services.AddScoped<ILogService, LogService>();
		services.AddScoped<IUploadServise, UploadServise>();
		services.AddScoped<IBaseInfoService, BaseInfoService>();
		services.AddScoped<ISoftSettingService, SoftSettingService>();
		services.AddScoped<IHomeMenuService, HomeMenuService>();
		services.AddScoped<IAccessManagmentService, AccessManagmentService>();
		services.AddScoped<ICustomeMenuService, CustomeMenuService>();
		services.AddScoped<IFileService, FileService>();
		return services;
	}



	public static ProjectSetting GetSiteSettings(this IServiceCollection services)
	{
		var provider = services.BuildServiceProvider();
		var siteSettingsOptions = provider.GetRequiredService<IOptionsSnapshot<ProjectSetting>>();
		var siteSettings = siteSettingsOptions.Value;
		if (siteSettings == null) throw new ArgumentNullException(nameof(siteSettings));
		return siteSettings;
	}

	public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
			var encryptionKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);

			var validationParameters = new TokenValidationParameters
			{
				ClockSkew = TimeSpan.Zero,
				RequireSignedTokens = true,
				ValidateIssuerSigningKey = true,
				RequireExpirationTime = false,
				ValidateLifetime = true,
				ValidateAudience = true,
				ValidateIssuer = true,
				IssuerSigningKey = new SymmetricSecurityKey(secretKey),
				ValidAudience = jwtSettings.Audience,
				ValidIssuer = jwtSettings.Issuer,
				TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
			};

			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = validationParameters;
			options.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = context =>
						{
							if (context.Exception != null)
								throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

							return Task.CompletedTask;
						},
				OnTokenValidated = async context =>
						{
							var signInManager = context.HttpContext.RequestServices.GetRequiredService<IApplicationSignInService>();
							var usermanager = context.HttpContext.RequestServices.GetRequiredService<IApplicationUserManager>();

							var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
							if (claimsIdentity.Claims?.Any() != true)
								context.Fail("This token has no claims.");


							var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
							if (!securityStamp.HasValue())
								context.Fail("This token has no security stamp");

							var userId = claimsIdentity.GetUserId();
							var user = await usermanager.FindByIdAsync(userId);

							if (user.SecurityStamp != securityStamp)
								context.Fail("Token security stamp is not valid.");

							var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
							if (validatedUser == null)
								context.Fail("Token security stamp is not valid.");

							await usermanager.UpdateLastLoginDateAsync(user);
						},
				//OnChallenge = context =>
				//{
				//    if (context.AuthenticateFailure != null)
				//        throw new AppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
				//    throw new AppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized);

				//}
			};
		});
	}

	public static void AddElmahCore(this IServiceCollection services, IConfiguration configuration, ProjectSetting siteSetting)
	{
		services.AddElmah<SqlErrorLog>(options =>
		{
			options.Path = siteSetting.SiteSetting.ElmahPath;
			options.ConnectionString = configuration.GetConnectionString("ElmahConnectionString");
			//options.CheckPermissionAction = httpContext => httpContext.User.Identity.IsAuthenticated;
		});
	}

	private static void setPasswordOptions(PasswordOptions identityOptionsPassword, ProjectSetting siteSettings)
	{
		identityOptionsPassword.RequireDigit = siteSettings.IdentitySettings.PasswordRequireDigit;
		identityOptionsPassword.RequireLowercase = siteSettings.IdentitySettings.PasswordRequireLowercase;
		identityOptionsPassword.RequireNonAlphanumeric = siteSettings.IdentitySettings.PasswordRequireNonAlphanumeric;
		identityOptionsPassword.RequireUppercase = siteSettings.IdentitySettings.PasswordRequireUppercase;
		identityOptionsPassword.RequiredLength = siteSettings.IdentitySettings.PasswordRequiredLength;
	}

}
