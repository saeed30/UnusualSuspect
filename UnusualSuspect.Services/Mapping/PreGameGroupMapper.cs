using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.ApiViewModels.InnerModels.PreGame;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.PreGame;

namespace UnusualSuspect.Services.Mapping;

public static class PreGameGroupMapper
{
  public static PreGameGroupDto ToPreGameGroupDto(this PreGameGroup value)
  {
    JoinedPreGame? owner = value.JoinedPreGames.FirstOrDefault(x => x.IsOwnerOfPreGroup);
    UserDto userDto = null;
    if (owner != null)
      userDto = owner.User.ToUserDto();
    return new PreGameGroupDto()
    {
      PreGameGroupId = value.Id,
      PreGameGroupStatusId = value.PreGameGroupStatusId,
      CalculatedJoinedUsers = value.CalculatedJoinedUsers,
      ReadyToGameTimeString = value.ReadyToGameTime.ToString(),
      GameTypeDto = value.GameType.ToGameTypeDto(),
      GameId = value.GameId,
      CreatedTimeString = value.CreatedTime.ToString(),
      OwnerDto = userDto
    };
  }
  public static IEnumerable<PreGameGroupDto> ToPreGameGroupDto(this IEnumerable<PreGameGroup> value)
  {
    return value.Select(x => x.ToPreGameGroupDto());

  }
  public static MyPreGameGroupsResponse ToMyPreGameGroupsResponse(this IEnumerable<PreGameGroup> value)
  {
    return new MyPreGameGroupsResponse()
    {
      PreGameGroups = value.ToPreGameGroupDto().ToList()
    };
  }

  public static PreGameDetailsViewModel ToPreGameDetailsViewModel(this PreGameGroup value)
  {
    return new PreGameDetailsViewModel()
    {
      GameTypeTitle = value.GameType.Title,
      PreGameGroupStatusTitle = value.PreGameGroupStatus.Title,
      PreGameGroupGetResponse = value.ToPreGameGroupDetailResponse()
    };
  }
  public static PreGameGroupGetResponse ToPreGameGroupDetailResponse(this PreGameGroup value)
  {
    return new PreGameGroupGetResponse()
    {
      PreGameGroupId = value.Id,
      PreGameGroupStatusId = value.PreGameGroupStatusId,
      CalculatedJoinedUsers = value.CalculatedJoinedUsers,
      GameTypeDto = value.GameType.ToGameTypeDto(),
      GameId = value.GameId,
      CreatedTime = value.CreatedTime,
      JoinedPreGame = value.JoinedPreGames.ToJoinedPreGameDto().ToList()
    };
  }
  public static IEnumerable<PreGameGroupGetResponse> ToPreGameGroupDetailResponse(this IEnumerable<PreGameGroup> value)
  {
    return value.Select(x => x.ToPreGameGroupDetailResponse());
  }
}