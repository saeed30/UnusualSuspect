using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class FriendRepository(IUnitOfWork uow, ILogger<FriendRepository> logger) :
  EfRepository<Friend>(uow, logger), IFriendRepository
{
  public IQueryable<ApplicationUser> GetFriends(int userId)
  {
    return BaseEntity.Where(x => x.UserId == userId).Select(x => x.FriendUser);
  }

  public async Task<bool> AddToFriendsAsync(ApplicationUser user, ApplicationUser friendUser, CancellationToken cancellationToken = default)
  {
    if (await BaseEntity.AnyAsync(x => x.UserId == user.Id && x.FriendUserId == friendUser.Id, cancellationToken))
      return false;
    BaseEntity.Add(new Friend()
    {
      User = user,
      FriendUser = friendUser,
      FriendshipStartTime = DateTime.Now
    });
    return true;
  }

  public async Task<bool> DeleteFriendAsync(ApplicationUser user, ApplicationUser friendUser, CancellationToken cancellationToken = default)
  {
    var f = await BaseEntity.Where(x => x.UserId == user.Id && x.FriendUserId == friendUser.Id)
      .ToListAsync(cancellationToken);
    BaseEntity.RemoveRange(f);
    return f.Any();
  }
}