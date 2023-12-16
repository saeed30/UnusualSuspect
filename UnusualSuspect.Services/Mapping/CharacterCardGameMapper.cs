using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class CharacterCardGameMapper
{
  public static GameCharacterDto ToGameCharacterDto(this CharacterCardGame value)
  {
    return new GameCharacterDto()
    {
      Title = value.CharacterCard.Title,
      CharacterId = value.CharacterCard.Id,
      Id = value.Id,
      IsMurderer = value.IsMurderer
    };
  }
  public static IEnumerable<GameCharacterDto> ToGameCharacterDto(this IEnumerable<CharacterCardGame> value)
  {
    return value.Select(x => x.ToGameCharacterDto());
  }
}