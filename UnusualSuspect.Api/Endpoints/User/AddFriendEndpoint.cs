using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class AddFriendEndpoint(IFriendService friendService, IUnitOfWork uow) : MyBaseEndpointAuthenticated
  .WithRequest<AddFriendRequest>
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/AddFriend")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(AddFriendRequest request, CancellationToken cancellationToken = default)
  {
    string phone = request.FriendMobileNumber;
    if (!PhoneNumberHelper.CheckAndFixPhoneNumber(ref phone))
    {
      return new ApiResultCommon(false, ApiResultStatusCode.NeedToRetry
        , "شماره همراه به درستی وارد نشده است");
    }
    var result = await friendService.AddToFriendsAsync(CurrentUser.UserId, phone, cancellationToken);
    if (result.Success)
    {
      if(result.Result)
        await uow.SaveChangesAsync(cancellationToken);
      return new ApiResultCommon(result.Result, ApiResultStatusCode.Success);
    }
    return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());

  }
}