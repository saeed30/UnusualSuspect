using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GemPackageRepository(IUnitOfWork uow, ILogger<GemPackageRepository> logger)
  : EfRepository<GemPackage, short>(uow, logger), IGemPackageRepository
{
  public IEnumerable<GemPackage> GetAllActive()
  {
    return BaseEntity.Where(x => x.IsActive);
  }

  public async Task<List<GemPackage>> GetAllActivePublicAsync(CancellationToken cancellationToken)
  {
    return await BaseEntity.Where(x => x.IsActive && x.IsPublic).ToListAsync(cancellationToken);
  }
}