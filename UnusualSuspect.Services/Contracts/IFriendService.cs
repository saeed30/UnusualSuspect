using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Services.Contracts
{
  public interface IFriendService
  {
    Task<UnusualSuspectServiceResult<GetFriendsResponse>> GetFriendListAsync(int userId, CancellationToken cancellationToken = default);
    Task<UnusualSuspectServiceResult<bool>> AddToFriendsAsync(int userId, string friendMobileNumber, CancellationToken cancellationToken = default);
    Task<UnusualSuspectServiceResult<bool>> DeleteFriend(int userId, int friendUserId, CancellationToken cancellationToken = default);
  }
}
