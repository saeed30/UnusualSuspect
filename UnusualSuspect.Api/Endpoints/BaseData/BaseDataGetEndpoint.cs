using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.BaseData;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.BaseData
{
  public class BaseDataGetEndpoint(IBaseDataService baseDataService) : MyBaseEndpointAuthenticated
    .WithoutRequest
    .WithActionResult<ApiResultCommon<BaseDataGetResponse>>
  {
  [HttpGet("api/[namespace]/BaseDataGet")]
    public override async Task<ActionResult<ApiResultCommon<BaseDataGetResponse>>> HandleAsync(CancellationToken cancellationToken = default)
    {
      UnusualSuspectServiceResult<BaseDataGetResponse> result = await baseDataService.GetBaseDataGetResponseAsync(CurrentUser.UserId, cancellationToken);
      return ReturnResult(result);
    }
  }
}
