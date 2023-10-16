using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Contracts;

public interface IFireBaseService
{
    Task<HttpResponseMessage> SendNotification(string usertokenorchannelname, string message, string body, string subtitle);
    Task<HttpResponseMessage> SendFcm(string message, string body, string subtitle);
    Task SendGroupDataMessageAsync(IEnumerable<string> fireBasetokens, object p);
    Task<HttpResponseMessage> SendData(string usertokenorchannelname, object data);
}
