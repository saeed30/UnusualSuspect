using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories
{
  public sealed class AvatarPackageUserRepository(IUnitOfWork uow, ILogger<AvatarPackageUserRepository> logger)
    : EfRepository<AvatarPackageUser>(uow, logger), IAvatarPackageUserRepository
  {
    public async Task<bool> OwnedByUserAsync(short packageId, int userId, CancellationToken cancellationToken = default)
    {
      return await BaseEntity.AnyAsync(x => x.UserId == userId && x.AvatarPackageId == packageId, cancellationToken);
    }
  }
}
