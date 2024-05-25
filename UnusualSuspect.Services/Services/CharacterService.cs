using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services;

public class CharacterService(ICharacterCardRepository characterCardRepository,
  IFileService fileService,
  IUnitOfWork uow) : ICharacterService
{
  public IQueryable<CharacterCard> GetAllCharacters()
  {
    return characterCardRepository.GetAllCharacterCards();
  }

  public CharacterCard AddCharacter(CharacterCard characterCard)
  {
    return characterCardRepository.Add(characterCard);
  }

  public async Task<UnusualSuspectServiceResult<bool>> UpdateCharacterAsync(CharacterCard characterCard, short originalId, CancellationToken cancellationToken = default)
  {
    if (characterCard.Id == originalId)
    {
      UpdateCharacter(characterCard);
      return new UnusualSuspectServiceResult<bool>(true);
    }
    CharacterCard? old = await characterCardRepository.GetByIdAsync(characterCard.Id, cancellationToken);
    if (old != null)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidCharacterCardId));
    CharacterCard? original = await characterCardRepository.GetByIdAsync(originalId, cancellationToken);
    if (original == null)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.CharacterCardIdAlreadyExists));
    await characterCardRepository.ExecuteUpdateByIdAsync(characterCard, originalId, cancellationToken);
    return new UnusualSuspectServiceResult<bool>(false);
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