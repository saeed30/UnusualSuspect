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
using Newtonsoft.Json;
using Quartz;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using ElmahCore;
using NetCore.AutoRegisterDi;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Exceptions;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.DataLayer.Contracts.Repository.TopRanking;
using UnusualSuspect.DataLayer.Repositories;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Identity;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.Services.JcoSecurity;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Settings;
using UnusualSuspect.DataLayer.Repositories.TopRanking;

namespace UnusualSuspect.IocConfig;

public static class ServiceCollectionExtensions
{
  private static IServiceCollection AddGameServices(this IServiceCollection services, IConfiguration configuration)
  {
    var assembliesToScan = new[]
    {
      //Assembly.GetExecutingAssembly(),
      Assembly.GetAssembly(typeof(GameService)),
      Assembly.GetAssembly(typeof(GameRepository))
    };
    services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
      .Where(c => c.Name.EndsWith("Service") || c.Name.EndsWith("Repository"))
      .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
    //services.AddScoped<IPreGameService, PreGameService>();
    //services.AddScoped<IGameService, GameService>();
    //services.AddScoped<IGemService, GemService>();
    //services.AddScoped<ICoinService, CoinService>();
    //services.AddScoped<IRankingService, RankingService>();
    //services.AddScoped<IQuestionService, QuestionService>();
    //services.AddScoped<ICharacterService, CharacterService>();
    //services.AddScoped<IScoreService, ScoreService>();
    //services.AddScoped<IFriendService, FriendService>();
    //services.AddScoped<IStickerService, StickerService>();
    //services.AddScoped<IJoinedPreGameRepository, JoinedPreGameRepository>();
    //services.AddScoped<IGameRepository, GameRepository>();
    //services.AddScoped<IParticipateRepository, ParticipateRepository>();
    //services.AddScoped<IScoreRepository, ScoreRepository>();
    //services.AddScoped<IPreGameGroupRepository, PreGameGroupRepository>();
    //services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
    //services.AddScoped<IGameTypeRepository, GameTypeRepository>();
    //services.AddScoped<ICharacterCardGameRepository, CharacterCardGameRepository>();
    //services.AddScoped<ICharacterCardRepository, CharacterCardRepository>();
    //services.AddScoped<ITopDayRankingRepository, TopDayRankingRepository>();
    //services.AddScoped<ITopWeekRankingRepository, TopWeekRankingRepository>();
    //services.AddScoped<ITopMonthRankingRepository, TopMonthRankingRepository>();
    //services.AddScoped<ITopTotalRankingRepository, TopTotalRankingRepository>();
    //services.AddScoped<IQuestionGameRepository, QuestionGameRepository>();
    //services.AddScoped<IQuestionRepository, QuestionRepository>();
    //services.AddScoped<IQuestionCharacterCardDefaultAnswerRepository, QuestionCharacterCardDefaultAnswerRepository>();
    //services.AddScoped<IGameCandidateRepository, GameCandidateRepository>();
    //services.AddScoped<IStickerRepository, StickerRepository>();
    //services.AddScoped<IFriendRepository, FriendRepository>();
    //services.AddScoped<IGemPackageUserRepository, GemPackageUserRepository>();
    //services.AddScoped<IGemPackageRepository, GemPackageRepository>();
    //services.AddScoped<ICoinPackageUserRepository, CoinPackageUserRepository>();
    //services.AddScoped<ICoinPackageRepository, CoinPackageRepository>();
    //services.AddScoped<INotificationService, NotificationService>();
    //services.AddScoped<IMemoryCacheService, MemoryCacheService>();
    //services.AddScoped<ITurnOfPlayService, TurnOfPlayService>();
    //services.AddScoped<IAvatarRepository, AvatarRepository>();
    //services.AddScoped<IAvatarPackageRepository, AvatarPackageRepository>();
    //services.AddScoped<IAvatarService, AvatarService>();
    //services.AddScoped<IStickerPackageRepository, StickerPackageRepository>();
    //services.AddScoped<ITurnOfPlayService, TurnOfPlayService>();
    //services.AddScoped<IReportUserService, ReportUserService>();
    //services.AddScoped<IReportUserRepository, ReportUserRepository>();
    //services.AddScoped<IReportUserTypeRepository, ReportUserTypeRepository>();

    //services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

    return services;
  }
  public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
  {
    var settings = GetSiteSettings(services);
    services.AddConfiguredDbContext(settings);

    services.AddIdentity<ApplicationUser, Role>(identityOptions =>
    {
      setPasswordOptions(identityOptions.Password, settings);
    })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    services.AddElmahCore(configuration, settings);
    AddJwtAuthentication(services, settings);
    AddQuartzHostedService(services, settings);
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
    services.AddScoped<ISmsLogRepository, SmsLogRepository>();
    services.AddScoped<IDapperRepository, DapperRepository>();
    services.AddGameServices(configuration);

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
    var settings = GetSiteSettings(services);
    services.AddElmahCore(configuration, settings);
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
    services.AddScoped<ISmsLogRepository, SmsLogRepository>();
    services.AddScoped<IDapperRepository, DapperRepository>();
    services.AddGameServices(configuration);
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

  public static void AddJwtAuthentication(this IServiceCollection services, ProjectSetting projectSettings)
  {
    var jwtSettings = projectSettings.JwtSettings;
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
        OnChallenge = context =>
        {
          if (projectSettings.IsTesting)
          {
            if (context.AuthenticateFailure != null)
              ElmahExtensions.RaiseError(new AppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null));
            else
              ElmahExtensions.RaiseError(new AppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized));
          }
          if (!context.Response.HasStarted)
          {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var payload = new
            {
              error = "Unauthorized",
              message = "You need a valid token to access this resource"
            };
            context.HandleResponse();
            return context.Response.WriteAsync(JsonConvert.SerializeObject(payload));
          }
          return Task.CompletedTask;
        },
        //OnForbidden = context =>
        //{
        //  if (!context.Response.HasStarted)
        //  {
        //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //    context.Response.ContentType = "text/plain";
        //    return context.Response.WriteAsync("Authentication forbidden.");
        //  }
        //  return Task.CompletedTask;
        //},
        OnMessageReceived = context =>
        {
          if (string.IsNullOrEmpty(context.Token))
          {
            if (context.HttpContext.Request.Path.StartsWithSegments("/GameHub"))
            {
              string? accessToken = context.Request.Headers["Authorization"];
              if (string.IsNullOrEmpty(accessToken))
                accessToken = context.Request.Query["access_token"];
              if (!string.IsNullOrEmpty(accessToken))
                context.Token = accessToken.Replace("Bearer ", "");
            }
          }
          return Task.CompletedTask;
        },
        //OnAuthenticationFailed = context =>
        //    {
        //      //if (context.Exception != null)
        //      //  throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);
        //      // Set the status code to 401
        //      if (!context.Response.HasStarted)
        //      {
        //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //        // Set the content type to plain text
        //        context.Response.ContentType = "text/plain";
        //        // Write the exception message to the response body
        //        //return context.Response.WriteAsync(context.Exception.Message);
        //        return context.Response.WriteAsync("Authentication failed.");
        //      }
        //      return Task.CompletedTask;
        //    },
        OnTokenValidated = async context =>
            {
              var signInManager = context.HttpContext.RequestServices.GetRequiredService<IApplicationSignInService>();
              var userManager = context.HttpContext.RequestServices.GetRequiredService<IApplicationUserManager>();

              var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
              if (claimsIdentity.Claims?.Any() != true)
                context.Fail("This token has no claims.");


              var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
              if (!securityStamp.HasValue())
                context.Fail("This token has no security stamp");

              var userId = claimsIdentity.GetUserId();
              var user = await userManager.FindByIdAsync(userId);

              if (user.SecurityStamp != securityStamp)
                context.Fail("Token security stamp is not valid.");

              var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
              if (validatedUser == null)
                context.Fail("Token security stamp is not valid.");

              //await usermanager.UpdateLastLoginDateAsync(user);
            }
      };
    });
  }

  public static void AddElmahCore(this IServiceCollection services, IConfiguration configuration, ProjectSetting siteSetting)
  {
    services.AddElmah<SqlErrorLog>(options =>
    {
      options.Path = siteSetting.SiteSetting.ElmahPath;
      options.ConnectionString = configuration.GetConnectionString("ElmahConnectionString");
      options.Filters.Add(new CustomErrorFilter());
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

public class CustomErrorFilter : IErrorFilter
{
  public void OnErrorModuleFiltering(object sender, ExceptionFilterEventArgs args)
  {
    if (args.Context is Microsoft.AspNetCore.Http.DefaultHttpContext context)
    {
      if (context.Response != null && context.Response.StatusCode == StatusCodes.Status404NotFound)
        args.Dismiss();
    }
  }
}