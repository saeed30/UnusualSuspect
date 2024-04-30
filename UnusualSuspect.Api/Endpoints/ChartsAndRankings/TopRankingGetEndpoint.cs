using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.ChartsAndRankings;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.ChartsAndRankings
{
  public sealed class TopRankingGetEndpoint(IRankingService rankingService) : MyBaseEndpointAuthenticated
    .WithoutRequest
    .WithActionResult<ApiResultCommon<TopRankingGetResponse>>

  {
  [HttpGet("api/[namespace]/TopRankingGet")]
    public override Task<ActionResult<ApiResultCommon<TopRankingGetResponse>>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      throw new System.NotImplementedException();
    }
  }
}
