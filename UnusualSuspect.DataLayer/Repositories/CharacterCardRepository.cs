using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class CharacterCardRepository
  (IUnitOfWork uow, ILogger<CharacterCardRepository> logger) : EfRepository<CharacterCard, short>(uow, logger),
    ICharacterCardRepository
{
	private readonly DbSet<CharacterCard> characterCard = uow.Set<CharacterCard>();

  public async Task<List<CharacterCard>> GetAllActiveCharacterCardsAsync(CancellationToken cancellationToken = default)
	{
		return await characterCard.Where(x=>x.IsActive).ToListAsync(cancellationToken);
	}

  public async Task<List<CharacterCard>> GetRandomActiveCharacterCardsAsync(int count, CancellationToken cancellationToken = default)
  {
    if (count < 1)
      throw new Exception("count should be more than 0");
    return await characterCard.Where(x => x.IsActive).OrderBy(r => Guid.NewGuid()).Take(count).ToListAsync(cancellationToken);
  }
}