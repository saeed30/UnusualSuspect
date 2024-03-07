using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class FriendRepository(IUnitOfWork uow, ILogger<FriendRepository> logger) : EfRepository<Friend>(uow, logger), IFriendRepository
{
  private readonly DbSet<Friend> friends = uow.Set<Friend>();
  public IQueryable<ApplicationUser> GetFriends(int userId)
  {
    return friends.Where(x => x.UserId == userId).Select(x => x.FriendUser);
  }

  public async Task<bool> AddToFriendsAsync(ApplicationUser user, ApplicationUser friendUser, CancellationToken cancellationToken = default)
  {
    if (await friends.AnyAsync(x => x.UserId == user.Id && x.FriendUserId == friendUser.Id, cancellationToken))
      return false;
    friends.Add(new Friend()
    {
      User = user,
      FriendUser = friendUser,
      FriendshipStartTime = DateTime.Now
    });
    return true;
  }

  public async Task<bool> DeleteFriendAsync(ApplicationUser user, ApplicationUser friendUser, CancellationToken cancellationToken = default)
  {
    var f = await friends.Where(x => x.UserId == user.Id && x.FriendUserId == friendUser.Id)
      .ToListAsync(cancellationToken);
    friends.RemoveRange(f);
    return f.Any();
  }
}