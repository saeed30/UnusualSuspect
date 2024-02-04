using Microsoft.AspNetCore.Identity;

namespace UnusualSuspect.ViewModels.Settings;

public sealed class SiteSetting
{
    public string ElmahPath { get; set; }
    public string SiteUrl { get; set; }
    public string EngineerImagePath { get; set; }
    public string UserImagePath { get; set; }
    public string EventImagePath { get; set; }
    public string ChatImagePath { get; set; }
    public string RecordImagePath { get; set; }
    public string EngineerChatImagePath { get; set; }

    public string Privacy { get; set; }
    public string AppVersion { get; set; }
    public string AppDownloadLink { get; set; }

    public string AllowExtention { get; set; }

    public PasswordOptions PasswordOptions { get; set; }
    public CookieOptions CookieOptions { get; set; }

    public SmsSetting SmsSetting { get; set; }

    public string EngineerImageFullPath { get { return SiteUrl + EngineerImagePath; } }
    public string UserImageFullPath { get { return SiteUrl + UserImagePath; } }
    public string EventImageFullPath { get { return SiteUrl + EventImagePath; } }
    public string ChatImagePathFullPath { get { return SiteUrl + ChatImagePath; } }
    public string RecordImagePathFullPath { get { return SiteUrl + RecordImagePath; } }
    public string EngineerChatImagePathFullPath { get { return SiteUrl + EngineerChatImagePath; } }
}
