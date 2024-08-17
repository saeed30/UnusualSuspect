using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class PreGameGroupRepository
  (IUnitOfWork uow, ILogger<PreGameGroupRepository> logger) : EfRepository<PreGameGroup>(uow, logger),
    IPreGameGroupRepository
{
  public async Task<IReadOnlyList<PreGameGroup>> GetByUserIdWithJoinedPreGameAsync(int userId, int maxNumberOfGameRequests = 50, CancellationToken cancellationToken = default)
  {
    return await BaseEntity
      .Include(x => x.GameType)
      .Include(x => x.JoinedPreGames)
      .ThenInclude(x => x.User)
      .Where(x => x.JoinedPreGames.Any(j => j.UserId == userId))
      .OrderByDescending(x => x.ReadyToGameTime).Take(maxNumberOfGameRequests).ToListAsync(cancellationToken);
  }

  public async Task<PreGameGroup?> GetByIdWithDetailAsync(int preGameGroupId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity
      .Include(x => x.PreGameGroupStatus)
      .Include(x => x.GameType)
      .Include(x => x.JoinedPreGames)
      .ThenInclude(x => x.User)
      .ThenInclude(x => x.Document)
      .FirstOrDefaultAsync(x => x.Id == preGameGroupId, cancellationToken);
  }
  public async Task<PreGameGroup?> GetByIdWithGameTypeAsync(int preGameGroupId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity
      .Include(x => x.GameType)
      .FirstOrDefaultAsync(x => x.Id == preGameGroupId, cancellationToken);
  }

  public async Task<PreGameGroup?> GetFirstExpiredPregameGroupWithDetailsAsync(int expireMinutes, CancellationToken cancellationToken)
  {
    if (expireMinutes < 0)
      return null;
    DateTime expireTime = DateTime.Now.AddMinutes(-expireMinutes);
    return await BaseEntity.Include(x => x.JoinedPreGames)
      .FirstOrDefaultAsync(x =>
        !x.GameId.HasValue && x.PreGameGroupStatusId != (int)PreGameGroupStatusEnum.Ready &&
        !x.ReadyToGameTime.HasValue && x.CreatedTime < expireTime,
      cancellationToken);
  }

  public async Task<List<PreGameGroup>> GetTopPreGameGroupByReadyTimeAsync(GameType gameType, int count, CancellationToken cancellationToken = default)
  {
    return await BaseEntity
      .Where(x => x.ReadyToGameTime != null && x.PreGameGroupStatusId == (short)PreGameGroupStatusEnum.Ready && x.GameTypeId == gameType.Id)
      .OrderBy(x => x.ReadyToGameTime).Take(count).ToListAsync(cancellationToken);
  }

  public IQueryable<PreGameGroup> GetAllPreGameGroupsWithDetailsWaitingForGame()
  {
    return BaseEntity
      .Include(x => x.PreGameGroupStatus)
      .Include(x => x.GameType)
      .Where(x => x.GameId == null);
  }

  public async Task<List<int>> ResetGroupsStatusAfterFinishingTheGameAsync(int gameId, CancellationToken cancellationToken = default)
  {
    List<PreGameGroup> pre = await BaseEntity.Where(x => x.GameId == gameId).ToListAsync(cancellationToken);
    foreach (PreGameGroup gameGroup in pre)
    {
      gameGroup.GameId = null;
      gameGroup.PreGameGroupStatusId = (short)PreGameGroupStatusEnum.NotReady;
      gameGroup.ReadyToGameTime = null;
    }
    return pre.Select(x => x.Id).ToList();
  }

}