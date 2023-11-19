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
}