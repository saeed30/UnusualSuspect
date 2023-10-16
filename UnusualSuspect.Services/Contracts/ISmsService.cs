using System.Threading.Tasks;

namespace UnusualSuspect.Services.Contracts;

public interface ISmsService
{
    Task<bool> SendSmsAsync(string Mobile,string Message);
}
