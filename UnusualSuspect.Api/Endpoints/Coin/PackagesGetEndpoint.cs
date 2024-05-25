using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Api.Endpoints.Coin;

public sealed class PackagesGetEndpoint(ICoinService coinService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResultCommon<PackagesGetResponse>>
{
  [HttpGet("api/[namespace]/PackagesGet")]
  public override async Task<ActionResult<ApiResultCommon<PackagesGetResponse>>> HandleAsync(
    CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<PackagesGetResponse> result = await coinService.GetPublicPackagesAsync(cancellationToken);
    return ReturnResult(result);
  }
}