using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ICharacterCardGameRepository : IAsyncRepository<CharacterCardGame>
{
  Task<List<CharacterCardGame>> GetAllGameCharacterCardsAsync(int gameId, CancellationToken cancellationToken = default);
}