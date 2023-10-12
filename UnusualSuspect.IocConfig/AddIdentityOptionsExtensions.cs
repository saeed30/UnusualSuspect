using School.DataLayer.Context;
using School.Entities.Identity;
using School.Services.Config;
using School.ViewModels.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace School.IocConfig;

public static class AddIdentityOptionsExtensions
{
    public static IServiceCollection AddIdentityOptions(
      this IServiceCollection services, ProjectSetting projectSetting)
    {
        if (projectSetting == null) throw new ArgumentNullException(nameof(projectSetting));

        services.AddIdentity<ApplicationUser, Role>(identityOptions =>
        {
            setPasswordOptions(identityOptions.Password, projectSetting.SiteSetting);
        })
            //.AddErrorDescriber<CustomIdentityErrorDescriber>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(identityOptionsCookies =>
        {
            var provider = services.BuildServiceProvider();
            setApplicationCookieOptions(provider, identityOptionsCookies, projectSetting.SiteSetting);
        });

        return services;
    }
    private static void setPasswordOptions(PasswordOptions identityOptionsPassword, SiteSetting siteSettings)
    {
        identityOptionsPassword.RequireDigit = siteSettings.PasswordOptions.RequireDigit;
        identityOptionsPassword.RequireLowercase = siteSettings.PasswordOptions.RequireLowercase;
        identityOptionsPassword.RequireNonAlphanumeric = siteSettings.PasswordOptions.RequireNonAlphanumeric;
        identityOptionsPassword.RequireUppercase = siteSettings.PasswordOptions.RequireUppercase;
        identityOptionsPassword.RequiredLength = siteSettings.PasswordOptions.RequiredLength;
    }
    private static void setApplicationCookieOptions(IServiceProvider provider, CookieAuthenticationOptions identityOptionsCookies, SiteSetting siteSettings)
    {
        identityOptionsCookies.Cookie.Name = siteSettings.CookieOptions.CookieName;
        identityOptionsCookies.Cookie.HttpOnly = true;
        identityOptionsCookies.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        identityOptionsCookies.Cookie.SameSite = SameSiteMode.Lax;
        identityOptionsCookies.Cookie.IsEssential = true; 
        identityOptionsCookies.ExpireTimeSpan = siteSettings.CookieOptions.ExpireTimeSpan;
        identityOptionsCookies.SlidingExpiration = siteSettings.CookieOptions.SlidingExpiration;
        identityOptionsCookies.LoginPath = siteSettings.CookieOptions.LoginPath;
        identityOptionsCookies.LogoutPath = siteSettings.CookieOptions.LogoutPath;
        identityOptionsCookies.AccessDeniedPath = siteSettings.CookieOptions.AccessDeniedPath;
    }

}
