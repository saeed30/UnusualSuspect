using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class CharacterCardRepository : EfRepository<CharacterCard, short>, ICharacterCardRepository
{
	private readonly DbSet<CharacterCard> characterCard;
	public CharacterCardRepository(IUnitOfWork uow, ILogger<CharacterCardRepository> logger) : base(uow, logger)
	{
		characterCard = uow.Set<CharacterCard>();

	}

	public async Task<List<CharacterCard>> GetAllActiveCharacterCardsAsync(CancellationToken cancellationToken = default)
	{
		return await characterCard.Where(x=>x.IsActive).ToListAsync(cancellationToken);
	}
}