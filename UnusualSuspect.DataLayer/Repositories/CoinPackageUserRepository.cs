using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class CoinPackageUserRepository(IUnitOfWork uow, ILogger<CoinPackageUserRepository> logger)
  : EfRepository<CoinPackageUser>(uow, logger), ICoinPackageUserRepository
{
  public async Task<int> GetSumAsync(int userId)
  {
    return await BaseEntity.Where(x => x.UserId == userId).SumAsync(x => x.Amount);
  }

  public async Task<bool> OwnedByUserAsync(short packageId, int userId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.AnyAsync(x => x.UserId == userId && x.CoinPackageId == packageId, cancellationToken);
  }
}