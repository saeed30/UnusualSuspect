using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.DataLayer.Repositories;

public class ApplicationUserRepository(IUnitOfWork uow,
    ILogger<ApplicationUserRepository> logger)
  : EfRepository<ApplicationUser>(uow, logger), IApplicationUserRepository
{
}