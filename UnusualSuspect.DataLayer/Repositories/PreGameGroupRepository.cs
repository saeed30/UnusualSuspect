using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class PreGameGroupRepository
  (IUnitOfWork uow, ILogger<PreGameGroupRepository> logger) : EfRepository<PreGameGroup>(uow, logger),
    IPreGameGroupRepository
{
	private readonly DbSet<PreGameGroup> preGameGroup = uow.Set<PreGameGroup>();

  public async Task<PreGameGroup?> GetByIdWithJoinedPreGameAsync(int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await preGameGroup.Include(x=>x.JoinedPreGames)
			.FirstOrDefaultAsync(x=>x.Id == preGameGroupId, cancellationToken);
	}

	public async Task<List<PreGameGroup>> GetTopPreGameGroupByReadyTimeAsync(GameType gameType, int count, CancellationToken cancellationToken = default)
	{
		return await preGameGroup
			.Where(x => x.ReadyToGameTime != null && x.PreGameGroupStatusId == (short)PreGameGroupStatusEnum.Ready && x.GameTypeId == gameType.Id)
			.OrderBy(x => x.ReadyToGameTime).Take(count).ToListAsync(cancellationToken);
	}
}