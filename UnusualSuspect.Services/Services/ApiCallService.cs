using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UnusualSuspect.ApiViewModels.Endpoints.LocalOnly;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Api.Cafebazzar;
using UnusualSuspect.ViewModels.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Services;

public sealed class ApiCallService(IJwtService iJwtService,
  IApplicationUserManager iApplicationUserManager,
  IOptionsSnapshot<ProjectSetting> setting,
  ILogger<ApiCallService> logger) : IApiCallService
{
  public async Task<UnusualSuspectServiceResult<ApiResultCommon>> ChangeGameStateAsync(ChangeGameStateRequest request, string username, CancellationToken cancellationToken = default)
  {
    var user = await iApplicationUserManager.FindByNameAsync(username);
    if (user == null)
      return new UnusualSuspectServiceResult<ApiResultCommon>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidUsername));
    using HttpClient httpClient = new HttpClient();
    AccessToken jwtToken = await iJwtService.GenerateAsync(user);
    // Set the JWT token in the request headers
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(jwtToken.token_type, jwtToken.access_token);

    // Make the POST request
    var response = await httpClient.PostAsJsonAsync($"{setting.Value.SiteSetting.ApiUrl}/api/LocalOnly/ChangeGameState",
      request, cancellationToken: cancellationToken);

    // Ensure the response is successful
    response.EnsureSuccessStatusCode();

    // Deserialize the response content
    var result = await response.Content.ReadFromJsonAsync<ApiResultCommon>(cancellationToken: cancellationToken);
    if (result == null)
    {
      logger.LogEvent(SystemEventType.ChangeGameStateApiCallFailed, user.Id, "", logLevel: LogLevel.Critical);
      return LogicErrorCode.InvalidApiResponse;
    }
    return new UnusualSuspectServiceResult<ApiResultCommon>(result);

  }

  public async Task<UnusualSuspectServiceResult<(bool, string?)>> CheckPaymentInCafebazaar(PaymentUser paymentUser, CancellationToken cancellationToken = default)
  {
    using HttpClient httpClient = new HttpClient();
    string packageName = "your.package.name";
    string productId = "your_product_id";
    string accessToken = "your_access_token";

    string url = $"https://pardakht.cafebazaar.ir/devapi/v2/api/validate/{packageName}/inapp/{productId}/purchases/{paymentUser.PurchaseToken}";

    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);
    if (response.IsSuccessStatusCode)
    {
      string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
      try
      {
        if (response.IsSuccessStatusCode)
        {
          PurchaseValidationResponse? validationResponse = JsonConvert.DeserializeObject<PurchaseValidationResponse>(responseBody);
          if (validationResponse == null)
          {
            logger.LogEvent(SystemEventType.ErrorOnDeserializingCafeBazzarResponse, paymentUser.UserId, responseBody, logLevel: LogLevel.Critical);
            return LogicErrorCode.InvalidApiResponse;
          }
          return new UnusualSuspectServiceResult<(bool, string?)>((true, null));
        }
        ErrorResponse? errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseBody);
        if (errorResponse == null)
        {
          logger.LogEvent(SystemEventType.ErrorOnDeserializingCafeBazzarResponse, paymentUser.UserId, responseBody, logLevel: LogLevel.Warning);
          return LogicErrorCode.InvalidApiResponse;
        }
        return new UnusualSuspectServiceResult<(bool, string?)>((false, errorResponse.ErrorDescription));
      }
      catch (Exception a)
      {
        logger.LogEvent(SystemEventType.ErrorOnDeserializingCafeBazzarResponse, paymentUser.UserId, responseBody, exception: a, logLevel: LogLevel.Error);
        return LogicErrorCode.InvalidApiResponse;
      }
    }
    logger.LogEvent(SystemEventType.ErrorOnDeserializingCafeBazzarResponse, paymentUser.UserId, $"Error: {response.StatusCode}", logLevel: LogLevel.Error);
    return LogicErrorCode.InvalidApiResponse;
  }

  public Task<UnusualSuspectServiceResult<(bool, string?)>> CheckPaymentInMyket(PaymentUser paymentUser, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}
