using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class DeleteFriendEndpoint(IFriendService friendService, IUnitOfWork uow) : MyBaseEndpointAuthenticated
  .WithRequest<DeleteFriendRequest>
  .WithActionResult<ApiResult>
{
  [HttpPost("api/[namespace]/DeleteFriend")]
  public override async Task<ActionResult<ApiResult>> HandleAsync(DeleteFriendRequest request, CancellationToken cancellationToken = default)
  {
    var result = await friendService.DeleteFriend(CurrentUser.UserId, request.FriendUserId, cancellationToken);
    if (result.Success)
    {
      if (result.Result)
        await uow.SaveChangesAsync(cancellationToken);
      return new ApiResult(result.Result, ApiResultStatusCode.Success);
    }
    return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
  }
}