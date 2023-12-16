using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Contracts;

public interface IPreGameService
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<PreGameGroup>> CreatePreGameGroup(int userId, short gameTypeId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<JoinedPreGame>> AddUserToPreGameGroup(int addingUserId, string username, int preGameGroupId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<JoinedPreGame>> AddUserToPreGameGroup(int addingUserId, int userId, int preGameGroupId, CancellationToken cancellationToken = default);
  Task RemoveFromAllUserPreGames(List<JoinedPreGame> joinedPreGame, int userId, CancellationToken cancellationToken = default);
  Task RemoveUserFromPreGameGroup(JoinedPreGame joinedPreGame, int userId, CancellationToken cancellationToken = default);
  Task RemovePreGameGroup(int preGameGroupId, CancellationToken cancellationToken = default);
  Task RecalculatePreGameGroupUsers(int preGameGroupId, short changeOnThisTransaction = 0, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatus(int userId, int preGameGroupId, ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default);
  Task CombineGroupsToStartGames(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> PreGameGroupChangeReadyToPlayAsync(int preGameGroupId, PreGameGroupStatusEnum preGameGroupStatusEnum, CancellationToken cancellationToken = default);
  IQueryable<PreGameGroup> GetAllPreGameGroupsWithDetailsWaitingForGame();
  Task<UnusualSuspectServiceResult<PreGameGroupGetResponse>> GetPreGameGroupDetail(int preGameGroupId, int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<MyPreGameGroupsResponse>> GetPreGameGroupByUserId(int userId);
}