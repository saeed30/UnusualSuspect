using Microsoft.EntityFrameworkCore;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;
public sealed class FriendService(IFriendRepository friendRepository, IApplicationUserManager userManager) : IFriendService
{
  public async Task<UnusualSuspectServiceResult<GetFriendsResponse>> GetFriendListAsync(int userId, CancellationToken cancellationToken = default)
  {
    return new UnusualSuspectServiceResult<GetFriendsResponse>(
        new GetFriendsResponse()
        {
          FriendsUserDto = (await friendRepository.GetFriends(userId).ToListAsync(cancellationToken)).ToGameUserDto().ToList()
        }
      );
  }

  public async Task<UnusualSuspectServiceResult<bool>> AddToFriendsAsync(int userId, string friendMobileNumber, CancellationToken cancellationToken = default)
  {
    var user = await userManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserId));
    var friendUser = await userManager.FindByNameAsync(friendMobileNumber);
    if (friendUser == null)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserIdForFriend));
    if (friendUser.Id == user.Id)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.CanNotAddOwnAsFriend));
    bool result = await friendRepository.AddToFriendsAsync(user, friendUser, cancellationToken);
    if (!result)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserIsAlreadyFriendWithTargetUser));
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async Task<UnusualSuspectServiceResult<bool>> DeleteFriend(int userId, int friendUserId, CancellationToken cancellationToken = default)
  {
    var user = await userManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserId));
    var friendUser = await userManager.FindByIdAsync(friendUserId.ToString());
    if (friendUser == null)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserIdForFriend));
    bool result = await friendRepository.DeleteFriendAsync(user, friendUser, cancellationToken);
    if(!result)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.SpecifiedUserIsNotFriend));
    return new UnusualSuspectServiceResult<bool>(true);
  }
}
