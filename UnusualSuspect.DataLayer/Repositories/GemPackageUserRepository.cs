using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GemPackageUserRepository(IUnitOfWork uow, ILogger<GemPackageUserRepository> logger)
  : EfRepository<GemPackageUser>(uow, logger), IGemPackageUserRepository
{
}