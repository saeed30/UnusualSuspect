using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Contracts;

public interface ICharacterService
{
  IQueryable<CharacterCard> GetAllCharacters();
  CharacterCard AddCharacter(CharacterCard characterCard);
  void UpdateCharacter(CharacterCard characterCard);
  void DeleteCharacter(CharacterCard characterCard);
  Task DeleteCharacterImageFileAsync(CharacterCard character);
}