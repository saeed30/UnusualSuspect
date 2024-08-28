using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Services;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public class GamePricesGetEndpoint(ISoftSettingService softSettingService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResultCommon<GamePricesGetResponse>>
{
  [HttpGet("api/[namespace]/GamePricesGet")]
  public override async Task<ActionResult<ApiResultCommon<GamePricesGetResponse>>> HandleAsync(CancellationToken cancellationToken = default)
  {
    var softSetting = await softSettingService.GetSoftSettingAsync(cancellationToken);
    return new ApiResultCommon<GamePricesGetResponse>(true, ApiResultStatusCode.Success, new GamePricesGetResponse()
    {
      JoinNormalGamePriceInCoin = softSetting.CoinCostToEnterPreGame,
      JoinNormalGamePriceInCoinForHost = softSetting.CoinCostToEnterPreGameForHost
    });
  }
}