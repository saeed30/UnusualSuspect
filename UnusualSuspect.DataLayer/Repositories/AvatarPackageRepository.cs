using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class AvatarPackageRepository(IUnitOfWork uow, ILogger<AvatarPackageRepository> logger)
  : EfRepository<AvatarPackage, short>(uow, logger), IAvatarPackageRepository
{
  public async Task<List<AvatarPackage>> GetAllActivePublicAsync(CancellationToken cancellationToken = default)
  {
    return await BaseEntity.Where(x => x.IsActive && x.IsPublic).ToListAsync(cancellationToken);
  }
}