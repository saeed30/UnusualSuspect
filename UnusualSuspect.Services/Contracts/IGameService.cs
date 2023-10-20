using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Contracts
{
	public interface IGameService
	{
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		Task<PreGameGroup> StartPreGameGroup(int userId, short gameTypeId, CancellationToken cancellationToken = default);
		Task<JoinedPreGame> AddUserToPreGameGroup(int userId, int preGameGroupId, CancellationToken cancellationToken = default);
		Task RemoveFromAllUserPreGames(List<JoinedPreGame> joinedPreGame, int userId, CancellationToken cancellationToken = default);
		Task RemoveUserFromPreGame(JoinedPreGame joinedPreGame, int userId, CancellationToken cancellationToken = default);
		Task RemoveUserFromPreGame(int preGameGroupId, CancellationToken cancellationToken = default);
		Task RecalculatePreGameGroupUsers(int preGameGroupId, CancellationToken cancellationToken = default);
	}
}
