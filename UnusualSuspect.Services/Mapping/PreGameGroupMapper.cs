using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.InnerModels.PreGame;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.PreGame;

namespace UnusualSuspect.Services.Mapping;

public static class PreGameGroupMapper
{
  public static PreGameGroupDto ToPreGameGroupDto(this PreGameGroup value)
  {
    return new PreGameGroupDto()
    {
      PreGameGroupId = value.Id,
      PreGameGroupStatusId = value.PreGameGroupStatusId,
      CalculatedJoinedUsers = value.CalculatedJoinedUsers,
      ReadyToGameTime = value.ReadyToGameTime,
      GameTypeId = value.GameTypeId,
      GameId = value.GameId,
      CreatedTime = value.CreatedTime
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
      GameTypeId = value.GameTypeId,
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