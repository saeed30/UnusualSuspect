using System.Threading.Tasks;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.Services.Contracts;

public interface ISmsService
{
    Task<bool> SendSmsAsync(string mobile, SmsMessageTextEnum message, List<string> parameters = null);
    Task<bool> SendSmsAsync(string mobile,string message);
    Task<SmsLog> SaveSmsSentLog(SmsLog smsLog, CancellationToken cancellationToken);
}
