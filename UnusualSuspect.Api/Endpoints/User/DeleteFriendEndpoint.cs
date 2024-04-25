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
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/DeleteFriend")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(DeleteFriendRequest request, CancellationToken cancellationToken = default)
  {
    var result = await friendService.DeleteFriend(CurrentUser.UserId, request.FriendUserId, cancellationToken);
    if (result.Success)
    {
      if (result.Result)
        await uow.SaveChangesAsync(cancellationToken);
      return new ApiResultCommon(result.Result, ApiResultStatusCode.Success);
    }
    return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
  }
}