using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Coin;

public sealed class PurchaseEndpoint(ICoinService coinService,
  IUnitOfWork uow,
  IBackgroundJobClient backgroundJobs) : MyBaseEndpointAuthenticated
  .WithRequest<int>
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/Purchase")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(int coinPackageId, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<(bool, PriceTypeEnum?)> result = await coinService.BuyPackagesAsync(coinPackageId, CurrentUser.UserId, false, cancellationToken);
    if (!result.Success)
      return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
    if (result.Result.Item1)
    {
      await uow.SaveChangesAsync(cancellationToken);
      backgroundJobs.Enqueue<IGemCoinCalculationJobsService>(job => job.RecalculateGemAndCoinByUserId(CurrentUser.UserId));
    }
    return new ApiResultCommon(result.Result.Item1, ApiResultStatusCode.Success);
  }
}