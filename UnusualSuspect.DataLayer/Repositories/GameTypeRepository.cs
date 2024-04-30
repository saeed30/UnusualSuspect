using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class GameTypeRepository
  (IUnitOfWork uow, ILogger<GameTypeRepository> logger) : EfRepository<GameType, short>(uow, logger),
    IGameTypeRepository
{
  public async Task<List<GameType>> GetActiveGameTypesAsync(CancellationToken cancellationToken = default)
	{
		return await baseEntity.Where(x => x.IsActive).ToListAsync(cancellationToken);
	}
}