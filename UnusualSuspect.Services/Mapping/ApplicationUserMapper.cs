using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Services.Mapping;

public static class ApplicationUserMapper
{
  public static UserDto ToUserDto(this ApplicationUser value)
  {
    return new UserDto()
    {
      Id = value.Id,
      NickName = value.NickName,
      Username = value.UserName,
      AvatarId = value.AvatarId ?? -1
    };
  }
  public static IEnumerable<UserDto> ToGameUserDto(this IEnumerable<ApplicationUser> value)
  {
    return value.Select(x => x.ToUserDto());
  }

}