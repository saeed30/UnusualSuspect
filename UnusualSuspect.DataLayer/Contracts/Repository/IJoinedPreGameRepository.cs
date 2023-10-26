using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository
{
	public interface IJoinedPreGameRepository : IAsyncRepository<JoinedPreGame>
	{
		Task<bool> UserExistsInAnyPreGameGroupAsync(int userId, CancellationToken cancellationToken = default);
		Task<bool> UserExistsInPreGameGroupAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
		Task<JoinedPreGame?> GetByUserIdPreGameGroupIdAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
		Task<bool> ExistsInPreGameGroupExceptUserAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
		Task ExecuteDeleteUserJoinedPreGameGroupAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
		Task<List<JoinedPreGame>> PreGameGroupOfUserAsync(int userId, CancellationToken cancellationToken = default);
		Task<int> UserCountJoinedPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default);
		Task<int> ExecuteDeleteAllJoinedPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default);
		Task<bool> AllJoinedPreGameGroupUsersAreReadyAsync(int preGameGroupId, CancellationToken cancellationToken = default);
	}
}
