using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
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
    return await baseEntity.AnyAsync(x => x.GameId == gameId && x.UserId == userId, cancellationToken);
  }

  public async Task<RoleCardEnum?> GetParticipantRoleAsync(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    Participate? participant =
      await baseEntity.FirstOrDefaultAsync(x => x.GameId == gameId && x.UserId == userId, cancellationToken);
    if (participant == null)
      return null;
    return (RoleCardEnum)participant.RoleCardId;
  }

  public async Task<List<Participate>> GetActiveParticipations(int userId, CancellationToken cancellationToken = default)
  {
    var result = await baseEntity.Where(x => 
        x.UserId == userId && x.IsActive && x.Game.FinishedTime == null)
      .ToListAsync(cancellationToken);
    if(result.Count > 1) 
      logger.LogCritical("User has more than one active game, userId: {userId}, gameIds: {gameIds}",
        userId, string.Join("-", result.Select(x=>x.Id)));
    return result;
  }
  public async Task<int> GetParticipantCountAsync(int gameId, CancellationToken cancellationToken = default)
  {
    return await baseEntity.CountAsync(x => x.GameId == gameId, cancellationToken);
  }

  public async Task<List<Participate>> GetGameActiveParticipantsAsync(int gameId, RoleCardEnum? roleCardId = null, CancellationToken cancellationToken = default)
  {
    if (roleCardId.HasValue)
      return await baseEntity.Where(x => x.IsActive && x.GameId == gameId && x.RoleCardId == (short)roleCardId.Value).ToListAsync(cancellationToken);
    return await baseEntity.Where(x => x.IsActive && x.GameId == gameId).ToListAsync(cancellationToken);
  }

  public async Task<bool> IsGameHasOtherActiveParticipantsAsync(int gameId, List<int> userIds)
  {
    return await baseEntity.AnyAsync(x => x.IsActive && x.GameId == gameId && !userIds.Contains(x.UserId));
  }
}