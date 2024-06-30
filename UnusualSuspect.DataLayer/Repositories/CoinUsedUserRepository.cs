using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class CoinUsedUserRepository(IUnitOfWork uow, ILogger<CoinUsedUserRepository> logger)
  : EfRepository<CoinUsedUser>(uow, logger), ICoinUsedUserRepository
{
  public async Task<int> GetSumAsync(int userId)
  {
    return await BaseEntity.Where(x => x.UserId == userId).SumAsync(x => x.Amount);
  }
}