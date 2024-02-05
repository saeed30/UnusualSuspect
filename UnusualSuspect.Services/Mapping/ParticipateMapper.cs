using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class ParticipateMapper
{
  public static GameParticipantDto ToGameParticipantDto(this Participate value)
  {
    return new GameParticipantDto()
    {
      Id = value.Id,
      OrderOfParticipation = value.OrderOfParticipation,
      GameRole = (GameRole)value.RoleCardId,
      UserDto = value.ApplicationUser.ToUserDto()
    };
  }
  public static List<GameParticipantDto> ToGameParticipantDto(this ICollection<Participate> value)
  {
    return value.Select(x => x.ToGameParticipantDto()).ToList();
  }

}