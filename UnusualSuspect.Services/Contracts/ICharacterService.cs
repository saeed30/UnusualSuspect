using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Contracts;

public interface ICharacterService
{
  IQueryable<CharacterCard> GetAllCharacters();
  CharacterCard AddCharacter(CharacterCard characterCard);
  void UpdateCharacter(CharacterCard characterCard);
  Task<UnusualSuspectServiceResult<bool>> UpdateCharacterAsync(CharacterCard characterCard, short originalId, CancellationToken cancellationToken = default);
  void DeleteCharacter(CharacterCard characterCard);
  Task DeleteCharacterImageFileAsync(CharacterCard character);
}