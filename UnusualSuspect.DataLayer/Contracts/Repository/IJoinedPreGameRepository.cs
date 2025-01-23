using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IJoinedPreGameRepository : IAsyncRepository<JoinedPreGame>
{
	Task<bool> UserExistsInAnyPreGameGroupAsync(int userId, CancellationToken cancellationToken = default);
	Task<bool> UserExistsInPreGameGroupAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
	Task<JoinedPreGame?> GetByUserIdPreGameGroupIdAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
	Task<bool> ExistsInPreGameGroupExceptUserAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
	Task ExecuteDeleteUserJoinedPreGameGroupAsync(int userId, CancellationToken cancellationToken = default);
	Task ExecuteDeleteUserJoinedPreGameGroupAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
	Task<List<JoinedPreGame>> JoinedPreGameOfUserAsync(int userId, CancellationToken cancellationToken = default);
	Task<List<JoinedPreGame>> JoinedPreGameOfPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default);
	Task<int> UserCountJoinedPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default);
	Task<bool> AllJoinedPreGameGroupUsersAreReadyAsync(int preGameGroupId, CancellationToken cancellationToken = default);
	Task ResetJoinedPreGameAfterFinishingTheGameAsync(List<int> preGameGroupIds, CancellationToken cancellationToken = default);
	Task<bool> IsGroupOwner(int preGameGroupId, int userId, CancellationToken cancellationToken = default);
  Task<bool> IsGroupMember(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
  Task<IEnumerable<JoinedPreGame>> GetAllOwnedByUserId(int userId, CancellationToken cancellationToken = default);
  Task<List<JoinedPreGame>> GetByUserIdAsync(int userId, ReadyToGameStatusEnum? readyToGameStatusEnum = null, CancellationToken cancellationToken = default);
  Task<bool> HaveDuplicateReadyJoinedPregameByPregameGroupIdAsync(int preGameGroupId, CancellationToken cancellationToken = default);
}