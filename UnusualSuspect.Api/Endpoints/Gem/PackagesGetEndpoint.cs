using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Gem;

public sealed class PackagesGetEndpoint(IGemService gemService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResultCommon<PackagesGetResponse>>
{
  [HttpGet("api/[namespace]/PackagesGet")]
  public override async Task<ActionResult<ApiResultCommon<PackagesGetResponse>>> HandleAsync(
    CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<PackagesGetResponse> result = await gemService.GetPublicPackagesAsync(cancellationToken);
    return ReturnResult(result);
  }
}