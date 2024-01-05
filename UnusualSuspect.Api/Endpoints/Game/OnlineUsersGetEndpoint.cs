using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public sealed class OnlineUsersGetEndpoint(IMemoryCacheService memoryCacheService) : MyBaseEndpointAuthenticated
  .WithRequest<int>
  .WithActionResult<ApiResult<OnlineUsersGetResponse>>
{
  [HttpGet("api/[namespace]/OnlineUsersGet")]
  public override async Task<ActionResult<ApiResult<OnlineUsersGetResponse>>> HandleAsync(int gameId, CancellationToken cancellationToken = default)
  {
    List<int> userIds = await memoryCacheService.GetSignalRGroupOnlineUsers(gameId.ToString());
    return new ApiResult<OnlineUsersGetResponse>(true, ApiResultStatusCode.Success, new OnlineUsersGetResponse()
    {
      GameId = gameId,
      UserIds = userIds
    });

  }
}