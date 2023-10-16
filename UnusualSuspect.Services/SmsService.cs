using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace UnusualSuspect.Services;

public class SmsService : ISmsService
{
    private readonly IOptionsSnapshot<ProjectSetting> _setting;
    public SmsService(
        IOptionsSnapshot<ProjectSetting> setting)
    {
        _setting = setting ?? throw new ArgumentNullException(nameof(setting));
    }

    public async Task<bool> SendSmsAsync(string Mobile, string Message)
    {
        return true;
    }
}
