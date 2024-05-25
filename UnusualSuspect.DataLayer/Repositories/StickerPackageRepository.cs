using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class StickerPackageRepository(IUnitOfWork uow, ILogger<StickerPackageRepository> logger)
  : EfRepository<StickerPackage, short>(uow, logger), IStickerPackageRepository
{
  public async Task<List<StickerPackage>> GetAllActivePublicAsync(CancellationToken cancellationToken)
  {
    return await baseEntity.Where(x => x.IsActive && x.IsPublic).ToListAsync(cancellationToken);
  }
}