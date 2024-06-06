using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Avatar;

public sealed class PackagesGetEndpoint(IAvatarService avatarService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResultCommon<PackagesGetResponse>>
{
  [HttpGet("api/[namespace]/PackagesGet")]
  public override async Task<ActionResult<ApiResultCommon<PackagesGetResponse>>> HandleAsync(CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<PackagesGetResponse> result = await avatarService.GetPublicPackagesAsync(CurrentUser.UserId, cancellationToken);
    return ReturnResult(result);
  }
}