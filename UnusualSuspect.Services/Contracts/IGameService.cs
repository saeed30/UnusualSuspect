using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Game;

namespace UnusualSuspect.Services.Contracts
{
	public interface IGameService
	{
		Task<UnusualSuspectServiceResult<GameGetResponse>> GetGameAsync(int gameId);
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
