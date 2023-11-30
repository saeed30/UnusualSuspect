using System.Threading;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ICharacterCardGameRepository : IAsyncRepository<CharacterCardGame>
{
  Task<List<CharacterCardGame>> GetAllGameCharacterCards(int gameId, CancellationToken cancellationToken = default);
}