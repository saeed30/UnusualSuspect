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
      IsMurderer = value.IsMurderer,
      ImageUrl = value.CharacterCard.ImageUrl
    };
  }
  public static List<GameCharacterDto> ToGameCharacterDto(this ICollection<CharacterCardGame> value)
  {
    return value.Select(x => x.ToGameCharacterDto()).ToList();
  }
}