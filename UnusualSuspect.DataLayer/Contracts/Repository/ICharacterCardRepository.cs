using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ICharacterCardRepository : IAsyncRepository<CharacterCard, short>
{
	Task<List<CharacterCard>> GetAllActiveCharacterCardsAsync(CancellationToken cancellationToken = default);

}