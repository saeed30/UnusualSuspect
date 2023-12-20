using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class PreGameGroupGetEndpoint(IPreGameService preGameService) : MyBaseEndpointAuthenticated
  .WithRequest<int>
  .WithActionResult<ApiResult<PreGameGroupGetResponse>>
{
  [HttpGet("api/[namespace]/{id}", Name = "[namespace]_[controller]")]
  public override async Task<ActionResult<ApiResult<PreGameGroupGetResponse>>> HandleAsync(int id,
    CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult <PreGameGroupGetResponse> result = await preGameService.GetPreGameGroupDetail(id, CurrentUser.UserId, cancellationToken);
    return ReturnResult(result);
  }
}