using UnusualSuspect.Common.Enums;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.Services.Contracts;

public interface ISmsService
{
  Task<bool> SendSmsAsync(string mobile, SmsMessageTextEnum message, List<string> parameters, CancellationToken cancellationToken = default);
  Task<bool> SendSmsAsync(string mobile, string message, CancellationToken cancellationToken = default);
  Task<bool> SendOtpAsync(string mobile, string code, int userId, CancellationToken cancellationToken = default);
  SmsLog SaveSmsSentLog(SmsLog smsLog);
}
