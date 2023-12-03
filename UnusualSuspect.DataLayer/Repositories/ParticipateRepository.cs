using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class ParticipateRepository
  (IUnitOfWork uow, ILogger<ParticipateRepository> logger) : EfRepository<Participate>(uow, logger),
    IParticipateRepository
{
  private readonly DbSet<Participate> participates = uow.Set<Participate>();

  public async Task<bool> IsGameParticipantAsync(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    return await participates.AnyAsync(x => x.GameId == gameId && x.UserId == userId, cancellationToken);
  }

  public async Task<RoleCardEnum?> GetParticipantRoleAsync(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    Participate? participant =
      await participates.FirstOrDefaultAsync(x => x.GameId == gameId && x.UserId == userId, cancellationToken);
    if (participant == null)
      return null;
    return (RoleCardEnum)participant.RoleCardId;
  }

  public async Task<Participate?> GetActiveParticipation(int userId, CancellationToken cancellationToken = default)
  {
    return await participates.FirstOrDefaultAsync(x => x.UserId == userId && x.Game.FinishedTime == null, cancellationToken);
  }
}