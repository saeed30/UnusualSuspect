using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.ApiViewModels.InnerModels.PreGame;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class JoinedPreGameMapper
{
  public static JoinedPreGameDto ToJoinedPreGameDto(this JoinedPreGame value)
  {
    return new JoinedPreGameDto()
    {
      IsOwnerOfPreGroup = value.IsOwnerOfPreGroup,
      JoinTimeString = value.JoinTime.ToString(),
      ReadyToGameStatus = (ReadyToGameStatusEnum)value.ReadyToGameStatusId,
      UserDto = new UserDto()
      {
        AvatarId = value.User.AvatarId ?? -1,
        Id = value.User.Id,
        NickName = value.User.NickName,
        Username = value.User.UserName
      }
    };
  }
  public static IEnumerable<JoinedPreGameDto> ToJoinedPreGameDto(this IEnumerable<JoinedPreGame> value)
  {
    return value.Select(x => x.ToJoinedPreGameDto());
  }

}