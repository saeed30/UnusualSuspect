using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class CoinPackageRepository(IUnitOfWork uow, ILogger<CoinPackageRepository> logger)
  : EfRepository<CoinPackage, short>(uow, logger), ICoinPackageRepository
{
  public async Task<List<CoinPackage>> GetAllActivePublicAsync(CancellationToken cancellationToken)
  {
    return await baseEntity.Where(x => x.IsActive && x.IsPublic).ToListAsync(cancellationToken);
  }
}