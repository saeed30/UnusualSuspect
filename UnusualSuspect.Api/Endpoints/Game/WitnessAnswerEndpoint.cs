using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game
{
  public class WitnessAnswerEndpoint(IGameService gameService,
		IMemoryCacheService memoryCacheService,
	  INotificationService notificationService) : MyBaseEndpointAuthenticated
    .WithRequest<WitnessAnswerRequest>
    .WithActionResult<ApiResult>
  {
  [HttpPost("api/[namespace]/WitnessAnswer")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(WitnessAnswerRequest request, CancellationToken cancellationToken = default)
    {
      UnusualSuspectServiceResult<bool> result = await gameService.SetWitnessAnswer(request.GameId, request.WitnessAnswer, request.QuestionId, CurrentUser.UserId, cancellationToken);
      if (!result.Success)
        return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
      await gameService.SaveChangesAsync(cancellationToken);
			memoryCacheService.ClearGameWithDetails(request.GameId);
      await notificationService.SendSignalToGameGroup(request.GameId, SignalCommands.WitnessAnswered,
        request.WitnessAnswer);
      return new ApiResult(true, ApiResultStatusCode.Success);

    }
  }
}
