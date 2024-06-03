using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Account;
using UnusualSuspect.ApiViewModels.Endpoints.Avatar;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Avatar
{
  public sealed class PurchaseEndpoint(IAvatarService avatarService, IUnitOfWork uow) : MyBaseEndpointAuthenticated
    .WithRequest<AvatarPurchaseRequest>
    .WithActionResult<ApiResultCommon>
  {
    public override async Task<ActionResult<ApiResultCommon>> HandleAsync(AvatarPurchaseRequest request, CancellationToken cancellationToken = default)
    {
      var result = await avatarService.BuyPackagesAsync(request, CurrentUser.UserId, cancellationToken);
      if (!result.Success)
        return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
      if (result.Result)
        await uow.SaveChangesAsync(cancellationToken);
      return new ApiResultCommon(result.Result, ApiResultStatusCode.Success);
    }
  }
}
