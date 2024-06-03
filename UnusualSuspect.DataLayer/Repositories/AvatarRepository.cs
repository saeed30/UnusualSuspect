using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class AvatarRepository(IUnitOfWork uow, ILogger<AvatarRepository> logger)
  : EfRepository<Avatar, short>(uow, logger), IAvatarRepository
{
  public Task<bool> UserOwnsPackage(short avatarId, int userId, CancellationToken cancellationToken = default)
  {
    return BaseEntity.AnyAsync(x=>x.IsActive)
  }
}