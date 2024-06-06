using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GemPackageUserRepository(IUnitOfWork uow, ILogger<GemPackageUserRepository> logger)
  : EfRepository<GemPackageUser>(uow, logger), IGemPackageUserRepository
{
  public async Task<int> GetSumAsync(int userId)
  {
    return await BaseEntity.Where(x => x.UserId == userId && x.IsActive).SumAsync(x => x.Amount);
  }

  public async Task<bool> OwnedByUserAsync(short packageId, int userId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.AnyAsync(x => x.UserId == userId && x.GemPackageId == packageId, cancellationToken);
  }
}