using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Services.Mapping;

public static class ApplicationUserMapper
{
  public static GameUserDto ToGameUserDto(this ApplicationUser value)
  {
    return new GameUserDto()
    {
      Id = value.Id,
      NickName = value.NickName,
      Username = value.UserName,
      DocumentGuidKey = value.Document?.GuidKey
    };
  }
  public static IEnumerable<GameUserDto> ToGameUserDto(this IEnumerable<ApplicationUser> value)
  {
    return value.Select(x => x.ToGameUserDto());
  }

}