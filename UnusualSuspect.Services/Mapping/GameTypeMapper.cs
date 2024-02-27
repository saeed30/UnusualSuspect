using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class GameTypeMapper
{
  public static GameTypeDto ToGameTypeDto(this GameType value)
  {
    return new GameTypeDto()
    {
      Id = value.Id,
      Name = value.Name,
      Title = value.Title,
      NumberOfPlayers = value.NumberOfPlayers,
      AllowUserToAddOtherUsers = value.AllowUserToAddOtherUsers
    };
  }
  public static IEnumerable<GameTypeDto> ToGameTypeDto(this IEnumerable<GameType> value)
  {
    return value.Select(x => x.ToGameTypeDto());
  }

}