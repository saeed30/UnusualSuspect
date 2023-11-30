using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class CharacterCardGameRepository(IUnitOfWork uow, ILogger<CharacterCardGameRepository> logger)
  : EfRepository<CharacterCardGame>(uow, logger), ICharacterCardGameRepository
{
  private readonly DbSet<CharacterCardGame> characterCardGame = uow.Set<CharacterCardGame>();
  public async Task<List<CharacterCardGame>> GetAllGameCharacterCards(int gameId, CancellationToken cancellationToken = default)
  {
    return await characterCardGame.Where(x =>x.GameId == gameId).ToListAsync(cancellationToken);
  }
}