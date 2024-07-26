using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using UnusualSuspect.ApiViewModels.Endpoints.LocalOnly;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Services;

public sealed class ApiCallService(IJwtService iJwtService,
  IApplicationUserManager iApplicationUserManager,
  IOptionsSnapshot<ProjectSetting> setting) : IApiCallService
{
  public async Task<UnusualSuspectServiceResult<ApiResultCommon>> ChangeGameStateAsync(ChangeGameStateRequest request, string username)
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
    var response = await httpClient.PostAsJsonAsync($"{setting.Value.SiteSetting.ApiUrl}/api/LocalOnly/ChangeGameState", request);

    // Ensure the response is successful
    response.EnsureSuccessStatusCode();

    // Deserialize the response content
    var result = await response.Content.ReadFromJsonAsync<ApiResultCommon>();
    if (result == null)
      return new UnusualSuspectServiceResult<ApiResultCommon>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidApiResponse));
    return new UnusualSuspectServiceResult<ApiResultCommon>(result);

  }
}