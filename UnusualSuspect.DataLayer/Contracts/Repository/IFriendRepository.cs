using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IFriendRepository : IAsyncRepository<Friend>
{
  IQueryable<ApplicationUser> GetFriends(int userId);
  Task<bool> AddToFriendsAsync(ApplicationUser user, ApplicationUser friendUser, CancellationToken cancellationToken = default);
  Task<bool> DeleteFriendAsync(ApplicationUser user, ApplicationUser friendUser, CancellationToken cancellationToken = default);
}