using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class CharacterCardGameRepository(IUnitOfWork uow, ILogger<CharacterCardGameRepository> logger)
  : EfRepository<CharacterCardGame>(uow, logger), ICharacterCardGameRepository
{
  public async Task<List<CharacterCardGame>> GetAllGameCharacterCardsAsync(int gameId, CancellationToken cancellationToken = default)
  {
    return await baseEntity.Where(x =>x.GameId == gameId).ToListAsync(cancellationToken);
  }
}