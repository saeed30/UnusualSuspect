using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ICharacterCardRepository : IAsyncRepository<CharacterCard, short>
{
	IQueryable<CharacterCard> GetAllCharacterCards();
	Task<List<CharacterCard>> GetAllActiveCharacterCardsAsync(CancellationToken cancellationToken = default);
	Task<List<CharacterCard>> GetRandomActiveCharacterCardsAsync(int count, CancellationToken cancellationToken = default);
  Task ExecuteUpdateByIdAsync(CharacterCard characterCard, short originalId, CancellationToken cancellationToken = default);
}