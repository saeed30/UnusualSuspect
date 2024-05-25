using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class CoinPackageUserRepository(IUnitOfWork uow, ILogger<CoinPackageUserRepository> logger)
  : EfRepository<CoinPackageUser>(uow, logger),ICoinPackageUserRepository
{
}