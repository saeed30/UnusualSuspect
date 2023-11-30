using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game
{
  public sealed class GameGetEndpoint(IGameService gameService) : MyBaseEndpointAuthenticated
    .WithRequest<int>
    .WithActionResult<ApiResult<GameGetResponse>>
  {
    [HttpGet("api/[namespace]/{id}", Name = "[namespace]_[controller]")]
    public override async Task<ActionResult<ApiResult<GameGetResponse>>> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
      UnusualSuspectServiceResult<GameGetResponse> game = await gameService.GetGameAsync(id, CurrentUser.UserId, cancellationToken);
      return ReturnResult(game);
    }
  }
}
