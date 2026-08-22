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

namespace UnusualSuspect.Api.Endpoints.Avatar;

public sealed class PurchaseEndpoint(IAvatarService avatarService,
	IUnitOfWork uow,
	IBackgroundJobClient backgroundJobs) : MyBaseEndpointAuthenticated
	.WithRequest<int>
	.WithActionResult<ApiResultCommon>
{
	[HttpPost("api/[namespace]/Purchase")]
	public override async Task<ActionResult<ApiResultCommon>> HandleAsync(int avatarPackageId, CancellationToken cancellationToken = default)
	{
		int userId = CurrentUser.UserId;
		UnusualSuspectServiceResult<(int?, PriceTypeEnum?)> result = await avatarService.BuyPackagesAsync(avatarPackageId, userId, cancellationToken);
		if (!result.Success)
			return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		if (result.Result.Item1.HasValue)
		{
			await uow.SaveChangesAsync(cancellationToken);
			if (result.Result.Item2.HasValue)
				if (result.Result.Item2.Value == PriceTypeEnum.Coin || result.Result.Item2.Value == PriceTypeEnum.Gem)
					backgroundJobs.Enqueue<IGemCoinCalculationJobsService>(job => job.RecalculateGemAndCoinByUserId(userId));
		}
		return new ApiResultCommon(result.Result.Item1.HasValue, ApiResultStatusCode.Success);
	}
}