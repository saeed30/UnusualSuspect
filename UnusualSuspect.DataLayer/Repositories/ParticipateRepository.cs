using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class ParticipateRepository
  (IUnitOfWork uow, ILogger<ParticipateRepository> logger) : EfRepository<Participate>(uow, logger),
    IParticipateRepository
{
  public async Task<bool> IsGameParticipantAsync(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.AnyAsync(x => x.GameId == gameId && x.UserId == userId, cancellationToken);
  }

  public async Task<RoleCardEnum?> GetParticipantRoleAsync(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    Participate? participant =
      await BaseEntity.FirstOrDefaultAsync(x => x.GameId == gameId && x.UserId == userId, cancellationToken);
    if (participant == null)
      return null;
    return (RoleCardEnum)participant.RoleCardId;
  }

  public async Task<List<Participate>> GetActiveParticipations(int userId, CancellationToken cancellationToken = default)
  {
    var result = await BaseEntity.Where(x =>
        x.UserId == userId && x.IsActive && x.Game.FinishedTime == null)
      .ToListAsync(cancellationToken);
    if (result.Count > 1)
      logger.LogEvent(SystemEventType.UserHasMoreThanOneActiveGame, userId,
        string.Join("-", result.Select(x => x.Id)), logLevel: LogLevel.Critical);
    return result;
  }
  public async Task<int> GetParticipantCountAsync(int gameId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.CountAsync(x => x.GameId == gameId, cancellationToken);
  }

  public async Task<List<Participate>> GetGameActiveParticipantsAsync(int gameId, RoleCardEnum? roleCardId = null, CancellationToken cancellationToken = default)
  {
    if (roleCardId.HasValue)
      return await BaseEntity.Where(x => x.IsActive && x.GameId == gameId && x.RoleCardId == (short)roleCardId.Value).ToListAsync(cancellationToken);
    return await BaseEntity.Where(x => x.IsActive && x.GameId == gameId).ToListAsync(cancellationToken);
  }

  public async Task<bool> IsGameHasOtherActiveParticipantsAsync(int gameId, List<int> userIds)
  {
    return await BaseEntity.AnyAsync(x => x.IsActive && x.GameId == gameId && !userIds.Contains(x.UserId));
  }
  public IQueryable<int> GetUsersGameIds(List<int> userIds)
  {
    return BaseEntity.Where(x => userIds.Contains(x.UserId) && x.IsActive && x.Game.FinishedTime == null).Select(x => x.Id);
  }
}