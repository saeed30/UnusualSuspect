using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Services;

namespace UnusualSuspect.Api.Endpoints.User
{
  public class DailyRewardGetEndpoint(ICoinService coinService,
    IGemService gemService,
    IUnitOfWork uow,
    ILogger<BaseDataService> logger) : MyBaseEndpointAuthenticated
    .WithoutRequest
    .WithActionResult<ApiResultCommon<DailyRewardGetResponse>>
  {
    [HttpGet("api/[namespace]/DailyRewardGet")]
    public override async Task<ActionResult<ApiResultCommon<DailyRewardGetResponse>>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
      if (!(await gemService.GivenTodayAward(CurrentUser.UserId, cancellationToken)) &&
          !(await coinService.GivenTodayAward(CurrentUser.UserId, cancellationToken)))
      {
        UnusualSuspectServiceResult<(int?, PriceTypeEnum?)> result1 =
          await gemService.BuyPackagesAsync(BaseGemPackageEnum.DailyAward, "", CurrentUser.UserId, StoreEnum.Unknown, true, cancellationToken);
        UnusualSuspectServiceResult<(int?, PriceTypeEnum?)> result2 =
          await coinService.BuyPackagesAsync(BaseCoinPackageEnum.DailyAward, CurrentUser.UserId, true, cancellationToken);
        if (result1.Success && result2.Success)
          await uow.SaveChangesAsync(cancellationToken);
        else
        {
          if (!result1.Success)
            logger.LogEvent(SystemEventType.DailyGemPackageEnumOnFailed, CurrentUser.UserId, result1.MainError.ToString(), logLevel: LogLevel.Critical);
          if (!result2.Success)
            logger.LogEvent(SystemEventType.DailyCoinPackageEnumOnFailed, CurrentUser.UserId, result2.MainError.ToString(), logLevel: LogLevel.Critical);
        }
        return new ApiResultCommon<DailyRewardGetResponse>(true, ApiResultStatusCode.Success,
          new DailyRewardGetResponse()
          {
            CoinReward = result2.Result.Item1 ?? 0,
            GemReward = result1.Result.Item1 ?? 0
          });
      }
      return new ApiResultCommon<DailyRewardGetResponse>(
        false, ApiResultStatusCode.LogicError,
        null,
        new UnusualSuspectErrorResult(LogicErrorCode.AlreadyRecievedDailyAward).ToString());

    }
  }
}
