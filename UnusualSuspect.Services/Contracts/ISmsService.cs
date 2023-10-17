using System.Threading.Tasks;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.Services.Contracts;

public interface ISmsService
{
    Task<bool> SendSmsAsync(string Mobile,string Message);
    Task<SmsLog> SaveSmsSentLog(SmsLog smsLog, CancellationToken cancellationToken);
}
