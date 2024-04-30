using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class FriendRepository(IUnitOfWork uow, ILogger<FriendRepository> logger) : EfRepository<Friend>(uow, logger), IFriendRepository
{
  public IQueryable<ApplicationUser> GetFriends(int userId)
  {
    return baseEntity.Where(x => x.UserId == userId).Select(x => x.FriendUser);
  }

  public async Task<bool> AddToFriendsAsync(ApplicationUser user, ApplicationUser friendUser, CancellationToken cancellationToken = default)
  {
    if (await baseEntity.AnyAsync(x => x.UserId == user.Id && x.FriendUserId == friendUser.Id, cancellationToken))
      return false;
    baseEntity.Add(new Friend()
    {
      User = user,
      FriendUser = friendUser,
      FriendshipStartTime = DateTime.Now
    });
    return true;
  }

  public async Task<bool> DeleteFriendAsync(ApplicationUser user, ApplicationUser friendUser, CancellationToken cancellationToken = default)
  {
    var f = await baseEntity.Where(x => x.UserId == user.Id && x.FriendUserId == friendUser.Id)
      .ToListAsync(cancellationToken);
    baseEntity.RemoveRange(f);
    return f.Any();
  }
}