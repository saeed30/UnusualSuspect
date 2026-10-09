using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services;
using UnusualSuspect.ApiViewModels.InnerModels;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Authorization;

namespace UnusualSuspect.Api.Endpoints.Coin;

public sealed class PackagesGetEndpoint(ICoinService coinService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResultCommon<PackagesGetResponse>>
{
	[AllowAnonymous, OutputCache(PolicyName = "PublicData", Duration = 20)]
	[HttpGet("api/[namespace]/PackagesGet")]
	public override async Task<ActionResult<ApiResultCommon<PackagesGetResponse>>> HandleAsync(
    CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<PackagesGetResponse> result = await coinService.GetPublicPackagesAsync(cancellationToken);
    return ReturnResult(result);
  }
}