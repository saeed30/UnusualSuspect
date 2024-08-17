using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class JoinedPreGameRepository
  (IUnitOfWork uow, ILogger<JoinedPreGameRepository> logger) : EfRepository<JoinedPreGame>(uow, logger),
    IJoinedPreGameRepository
{
  public async Task<bool> AllJoinedPreGameGroupUsersAreReadyAsync(int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return !await BaseEntity.AnyAsync(x => x.PreGameGroupId == preGameGroupId && x.ReadyToGameStatusId != (short)ReadyToGameStatusEnum.Ready, cancellationToken: cancellationToken);
	}

  public async Task ResetJoinedPreGameAfterFinishingTheGameAsync(List<int> preGameGroupIds, CancellationToken cancellationToken = default)
  {
	  var joinedPreGames = await BaseEntity.Where(x => preGameGroupIds.Contains(x.PreGameGroupId)).ToListAsync(cancellationToken);
	  foreach (JoinedPreGame preGame in joinedPreGames)
		  preGame.ReadyToGameStatusId = (short)ReadyToGameStatusEnum.Notified;
  }

	public async Task ExecuteDeleteUserJoinedPreGameGroupAsync(int userId, CancellationToken cancellationToken = default)
	{
		await BaseEntity.Where(x => x.UserId == userId).ExecuteDeleteAsync(cancellationToken);
	}

	public async Task ExecuteDeleteUserJoinedPreGameGroupAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
	{
		await BaseEntity.Where(x => x.UserId == userId && x.PreGameGroupId == preGameGroupId).ExecuteDeleteAsync(cancellationToken);
	}

	public async Task<bool> ExistsInPreGameGroupExceptUserAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await BaseEntity.AnyAsync(x => x.UserId != userId && x.PreGameGroupId == preGameGroupId, cancellationToken);
	}

	public async Task<JoinedPreGame?> GetByUserIdPreGameGroupIdAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await BaseEntity.FirstOrDefaultAsync(x =>
			x.UserId == userId && x.PreGameGroupId == preGameGroupId, cancellationToken);
	}
	public async Task<List<JoinedPreGame>> JoinedPreGameOfUserAsync(int userId, CancellationToken cancellationToken = default)
	{
		return await BaseEntity.Include(c => c.PreGameGroup)
			.Where(x => x.UserId == userId)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<JoinedPreGame>> JoinedPreGameOfPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await BaseEntity.Include(c => c.PreGameGroup)
			.Where(x => x.PreGameGroupId == preGameGroupId)
			.ToListAsync(cancellationToken);
	}

	public async Task<int> UserCountJoinedPreGameGroupAsync(int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await BaseEntity.CountAsync(x => x.PreGameGroupId == preGameGroupId, cancellationToken);
	}

	public async Task<bool> UserExistsInAnyPreGameGroupAsync(int userId, CancellationToken cancellationToken = default)
	{
		return await BaseEntity.AnyAsync(x => x.UserId == userId, cancellationToken);
	}

	public async Task<bool> UserExistsInPreGameGroupAsync(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
	{
		return await BaseEntity.AnyAsync(x => x.UserId == userId && x.PreGameGroupId == preGameGroupId, cancellationToken);
	}
	public async Task<bool> IsGroupOwner(int preGameGroupId, int userId, CancellationToken cancellationToken = default)
	{
		return await BaseEntity.AnyAsync(x => x.Id == preGameGroupId && x.UserId == userId &&
                                          x.IsOwnerOfPreGroup, cancellationToken);
	}

  public async Task<bool> IsGroupMember(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.AnyAsync(x => x.Id == preGameGroupId && x.UserId == userId, cancellationToken);
  }

  public async Task<IEnumerable<JoinedPreGame>> GetAllOwnedByUserId(int userId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.Where(x => x.UserId == userId && x.IsOwnerOfPreGroup).ToListAsync(cancellationToken);
  }

  public async Task<List<JoinedPreGame>> GetByUserIdAsync(int userId, ReadyToGameStatusEnum? readyToGameStatusEnum = null,
    CancellationToken cancellationToken = default)
  {
		if(readyToGameStatusEnum.HasValue)
      return await BaseEntity.Where(x => x.UserId == userId && x.ReadyToGameStatusId == (short)readyToGameStatusEnum.Value).ToListAsync(cancellationToken);
    return await BaseEntity.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
  }
}