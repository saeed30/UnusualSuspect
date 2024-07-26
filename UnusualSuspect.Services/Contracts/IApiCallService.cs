using UnusualSuspect.ApiViewModels.Endpoints.LocalOnly;
using UnusualSuspect.Common.Models;

namespace UnusualSuspect.Services.Contracts;

public interface IApiCallService
{
  Task<UnusualSuspectServiceResult<ApiResultCommon>> ChangeGameStateAsync(ChangeGameStateRequest request,
    string username);

}