using Microsoft.AspNetCore.Identity;

namespace UnusualSuspect.ViewModels.Settings;

public sealed class SiteSetting
{
    public string ElmahPath { get; set; }
    public string SiteUrl { get; set; }
    public string ApiUrl { get; set; }
    public string UserImagePath { get; set; }
    public string Privacy { get; set; }
    public string AppVersion { get; set; }
    public string AllowExtention { get; set; }

    public PasswordOptions PasswordOptions { get; set; }
    public CookieOptions CookieOptions { get; set; }

    public SmsSetting SmsSetting { get; set; }

    public string UserImageFullPath { get { return SiteUrl + UserImagePath; } }
}
