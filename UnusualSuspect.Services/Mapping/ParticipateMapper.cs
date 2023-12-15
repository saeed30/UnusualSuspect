using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
      GameUserDto = value.ApplicationUser.ToGameUserDto()
    };
  }
  public static IEnumerable<GameParticipantDto> ToGameParticipantDto(this IEnumerable<Participate> value)
  {
    return value.Select(x => x.ToGameParticipantDto());
  }

}