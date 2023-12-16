using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels.PreGame;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping
{
  public static class JoinedPreGameMapper
  {
    public static JoinedPreGameDto ToJoinedPreGameDto(this JoinedPreGame value)
    {
      return new JoinedPreGameDto()
      {
        UserId = value.UserId,
        IsOwnerOfPreGroup = value.IsOwnerOfPreGroup,
        JoinTime = value.JoinTime,
        ReadyToGameStatus = (ReadyToGameStatusEnum)value.ReadyToGameStatusId,
        DocumentGuidKey = value.User.Document?.GuidKey,
        NickName = value.User.NickName
      };
    }
    public static IEnumerable<JoinedPreGameDto> ToJoinedPreGameDto(this IEnumerable<JoinedPreGame> value)
    {
      return value.Select(x => x.ToJoinedPreGameDto());
    }

  }
}
