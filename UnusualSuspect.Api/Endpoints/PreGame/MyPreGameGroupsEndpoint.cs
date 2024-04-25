using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class MyPreGameGroupsEndpoint(IPreGameService preGameService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResultCommon<MyPreGameGroupsResponse>>
{
  [HttpGet("api/[namespace]/MyPreGameGroups", Name = "[namespace]_[controller]_MyPreGameGroups")]
  public override async Task<ActionResult<ApiResultCommon<MyPreGameGroupsResponse>>> HandleAsync(CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<MyPreGameGroupsResponse> result = await preGameService.GetPreGameGroupByUserId(CurrentUser.UserId);
    return ReturnResult(result);
  }
}