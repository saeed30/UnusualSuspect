using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Gem;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Gem;

public sealed class PurchaseEndpoint(IGemService gemService,
  IUnitOfWork uow,
  IBackgroundJobClient backgroundJobs) : MyBaseEndpointAuthenticated
  .WithRequest<GemPurchaseRequest>
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/Purchase")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(GemPurchaseRequest request, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<(bool, PriceTypeEnum?)> result = await gemService.BuyPackagesAsync(
      request.GemPackageId, request.PurchaseToken, CurrentUser.UserId, false, cancellationToken);
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