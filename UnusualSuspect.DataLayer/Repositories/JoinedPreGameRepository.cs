using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class JoinedPreGameRepository : EfRepository<JoinedPreGame>, IJoinedPreGameRepository
{
	private readonly IUnitOfWork uow;
	private readonly ILogger<EfRepository<JoinedPreGame>> logger;
	private readonly DbSet<JoinedPreGame> joinedPreGame;

	public JoinedPreGameRepository(IUnitOfWork uow, ILogger<EfRepository<JoinedPreGame>> logger) : base(uow, logger)
	{
		this.uow = uow;
		joinedPreGame = this.uow.Set<JoinedPreGame>();
		this.logger = logger;
	}

	public async Task<bool> AllJoinedPreGameGroupUsersAreReadyAsync(int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return !await joinedPreGame.AnyAsync(x => x.PreGameGroupId == preGameGroupId && x.ReadyToGameStatusId != (short)ReadyToGameStatusEnum.Ready, cancellationToken: cancellationToken);
	}

	public async Task<int> ExecuteDeleteAllJoinedPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await joinedPreGame.Where(x => x.PreGameGroupId == preGameGroupId).ExecuteDeleteAsync(cancellationToken);
	}
	public async Task ExecuteDeleteUserJoinedPreGameGroupAsync(int userId, CancellationToken cancellationToken = default)
	{
		await joinedPreGame.Where(x => x.UserId == userId).ExecuteDeleteAsync(cancellationToken);
	}

	public async Task ExecuteDeleteUserJoinedPreGameGroupAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
	{
		await joinedPreGame.Where(x => x.UserId == userId && x.PreGameGroupId == preGameGroupId).ExecuteDeleteAsync(cancellationToken);
	}

	public async Task<bool> ExistsInPreGameGroupExceptUserAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await joinedPreGame.AnyAsync(x => x.UserId != userId && x.PreGameGroupId == preGameGroupId, cancellationToken);
	}

	public async Task<JoinedPreGame?> GetByUserIdPreGameGroupIdAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await joinedPreGame.FirstOrDefaultAsync(x =>
			x.UserId == userId && x.PreGameGroupId == preGameGroupId, cancellationToken);
	}
	public async Task<List<JoinedPreGame>> JoinedPreGameOfUserAsync(int userId, CancellationToken cancellationToken = default)
	{
		return await joinedPreGame.Include(c => c.PreGameGroup)
			.Where(x => x.UserId == userId)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<JoinedPreGame>> JoinedPreGameOfPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await joinedPreGame.Include(c => c.PreGameGroup)
			.Where(x => x.PreGameGroupId == preGameGroupId)
			.ToListAsync(cancellationToken);
	}

	public async Task<int> UserCountJoinedPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await joinedPreGame.CountAsync(x => x.PreGameGroupId == preGameGroupId, cancellationToken);
	}

	public async Task<bool> UserExistsInAnyPreGameGroupAsync(int userId, CancellationToken cancellationToken = default)
	{
		return await joinedPreGame.AnyAsync(x => x.UserId == userId, cancellationToken);
	}

	public async Task<bool> UserExistsInPreGameGroupAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await joinedPreGame.AnyAsync(x => x.UserId == userId && x.PreGameGroupId == preGameGroupId, cancellationToken);
	}

}