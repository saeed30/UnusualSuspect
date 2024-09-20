using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.Options;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts.Repository;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Repositories;

namespace UnusualSuspect.Services.Services;

public class SmsService(IOptionsSnapshot<ProjectSetting> setting,
    ISmsLogRepository smsLogRepository,
    ILogger<SmsService> logger,
    ISoftSettingService softSettingService)
  : ISmsService
{
  public SmsLog SaveSmsSentLog(SmsLog smsLog)
  {
    return smsLogRepository.Add(smsLog);
  }

  public async Task<bool> SendSmsAsync(string mobile, SmsMessageTextEnum message, List<string> parameters, CancellationToken cancellationToken = default)
  {
    string msg = GetMessageFromEnum(message, parameters);
    return await SendSmsAsync(mobile, msg, cancellationToken);
  }

  public async Task<bool> SendOtpAsync(string mobile, string code, int userId, CancellationToken cancellationToken = default)
  {
    string responseBody = await SmsWebserviceComSendOtpAsync(mobile, code, cancellationToken);
    //smsLogRepository.Add(new SmsLog()
    //{
    //  DateTimeAddedToQueue = DateTime.Now,
    //  DateTimeSent = DateTime.Now,
      
    //});
    logger.LogEvent(SystemEventType.Login, userId, responseBody, logLevel: LogLevel.Information);
    return true;
  }
  public async Task<bool> SendSmsAsync(string mobile, string message, CancellationToken cancellationToken = default)
  {

    return true;
  }
  private string GetMessageFromEnum(SmsMessageTextEnum message, List<string> parameters)
  {
    string msg = message.ToDisplay();
    if (parameters == null || parameters.Count == 0)
      return msg;
    return string.Format(msg, parameters);
  }

  #region sms-webservice.com
  private async Task<string> SmsWebserviceComSendOtpAsync(string mobile, string code, CancellationToken cancellationToken = default)
  {
    var setting = await softSettingService.GetSoftSettingAsync(cancellationToken);
    string apiKey = setting.SmsProviderApiKey;//"268091-bdd186bb39f6461685f91088568704e4";
    string templateKey = "GameLogin";
    string p1 = code;
    string p2 = "";
    string p3 = "";
    string url = $"https://api.sms-webservice.com/api/V3/SendTokenSingle?ApiKey={apiKey}&TemplateKey={templateKey}&Destination={mobile}&p1={p1}&p2={p2}&p3={p3}";

    using var client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Add("Accept", "application/json");

    var response = await client.SendAsync(request, cancellationToken);
    return await response.Content.ReadAsStringAsync(cancellationToken);
  }
  #endregion sms-webservice.com
}
