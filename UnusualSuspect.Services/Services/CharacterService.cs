using Nancy.Validation;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services;

public class CharacterService(ICharacterCardRepository characterCardRepository, IFileService fileService) : ICharacterService
{
  public IQueryable<CharacterCard> GetAllCharacters()
  {
    return characterCardRepository.GetAllCharacterCards();
  }

  public CharacterCard AddCharacter(CharacterCard characterCard)
  {
    return characterCardRepository.Add(characterCard);
  }

  public void UpdateCharacter(CharacterCard characterCard)
  {
    characterCardRepository.Update(characterCard);
  }

  public void DeleteCharacter(CharacterCard characterCard)
  {
    characterCardRepository.Delete(characterCard);
  }

  public async Task DeleteCharacterImageFileAsync(CharacterCard character)
  {
    var model = await characterCardRepository.GetByIdAsync(character.Id);
    if(model == null || string.IsNullOrWhiteSpace(model.ImageUrl))
      return;
    fileService.DeleteFile(model.ImageUrl);
  }
}