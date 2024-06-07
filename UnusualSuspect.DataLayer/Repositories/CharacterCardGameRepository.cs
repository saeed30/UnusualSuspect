using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class CharacterCardGameRepository(IUnitOfWork uow, ILogger<CharacterCardGameRepository> logger)
  : EfRepository<CharacterCardGame>(uow, logger), ICharacterCardGameRepository
{
  public async Task<List<CharacterCardGame>> GetAllGameCharacterCardsAsync(int gameId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.Where(x =>x.GameId == gameId).ToListAsync(cancellationToken);
  }

  public async Task<CharacterCardGame?> GetMurderer(int gameId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.FirstOrDefaultAsync(x => x.GameId == gameId && x.IsMurderer, cancellationToken);
  }
}