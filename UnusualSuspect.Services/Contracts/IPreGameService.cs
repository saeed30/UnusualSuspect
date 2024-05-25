using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.PreGame;

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
  Task RecalculatePreGameGroupUsers(PreGameGroup preGameGroup, short changeOnThisTransaction = 0, CancellationToken cancellationToken = default);
  Task RecalculatePreGameGroupUsers(int preGameGroupId, short changeOnThisTransaction = 0, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> UnreadyAllUserReadyPreGameGroups(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatus(int userId, int preGameGroupId, ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatus(int userId, PreGameGroup preGameGroup, ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatus(JoinedPreGame joinedPreGame, ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default);
  UnusualSuspectServiceResult<bool> ChangeUserReadyStatus(JoinedPreGame joinedPreGame, PreGameGroup preGameGroup, ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default);
  Task CombineGroupsToStartGames(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> PreGameGroupChangeReadyToPlayAsync(int preGameGroupId, PreGameGroupStatusEnum preGameGroupStatusEnum, CancellationToken cancellationToken = default);
  IQueryable<PreGameGroup> GetAllPreGameGroupsWithDetailsWaitingForGame();
  Task<UnusualSuspectServiceResult<PreGameDetailsViewModel>> GetPreGameGroupViewModel(int preGameGroupId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<PreGameGroup>> GetPreGameGroupDetail(int preGameGroupId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<PreGameGroupGetResponse>> GetPreGameGroupResponseDetail(int preGameGroupId, int callerUserId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<PreGameGroupGetResponse>> GetPreGameGroupResponseDetail(int preGameGroupId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<MyPreGameGroupsResponse>> GetPreGameGroupByUserId(int userId);
  Task<UnusualSuspectServiceResult<bool>> ExitFromPreGameGroup(int preGameGroupId, int? userIdToExit, int currentUserId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> IsMemberOfPregameGroup(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
}