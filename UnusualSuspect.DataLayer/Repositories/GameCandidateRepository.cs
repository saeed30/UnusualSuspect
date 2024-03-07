using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.DataLayer.Contracts;
using System.Threading;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GameCandidateRepository(IUnitOfWork uow, ILogger<GameCandidateRepository> logger/*, IMemoryCacheService memoryCacheService*/)
  : EfRepository<GameCandidate>(uow, logger)
    , IGameCandidateRepository
{
  private readonly DbSet<GameCandidate> gameCandidates = uow.Set<GameCandidate>();
  public async Task<int> ExecuteDeleteAllGameCandidatesAsync(int gameId, CancellationToken cancellationToken = default)
  {
    return await gameCandidates.Where(x => x.GameId == gameId).ExecuteDeleteAsync(cancellationToken);
  }

  public async Task DeleteAllGameCandidatesAsync(int gameId, CancellationToken cancellationToken = default)
  {
    gameCandidates.RemoveRange(await gameCandidates.Where(x => x.GameId == gameId).ToListAsync(cancellationToken));
  }

  public async Task<List<GameCandidate>> GetAllGameCandidatesAsync(int gameId, CancellationToken cancellationToken = default)
  {
    //first get from cache
    return await gameCandidates.Where(x => x.GameId == gameId).ToListAsync(cancellationToken);
  }

  public async Task DeleteUserCandidatesInGameAsync(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    var candidates = await gameCandidates.Where(x => x.GameId == gameId && x.UserId == userId).ToListAsync(cancellationToken);
    gameCandidates.RemoveRange(candidates);
    //reload cache
  }

  public async Task ChangeUserCandidateInGameAsync(int gameId, int userId, short characterCardId,
    CancellationToken cancellationToken = default)
  {
    var candidates = await gameCandidates.Where(x => x.GameId == gameId && x.UserId == userId).ToListAsync(cancellationToken);
    if (!candidates.Any())
      Add(new GameCandidate()
      {
        GameId = gameId,
        UserId = userId,
        CharacterCardId = characterCardId,
        DateTimeAdded = DateTime.Now
      });
    else if (candidates.Count == 1)
    {

      var candidate = candidates.First();
      if (candidate.CharacterCardId == characterCardId)
        return;
      candidate.CharacterCardId = characterCardId;
      candidate.DateTimeAdded = DateTime.Now;
    }
    else
    {
      logger.LogWarning("User has multiple candidates in a game. userId: {userId} - gameId: {gameId}", userId, gameId);
      var candidate = candidates.FirstOrDefault(x => x.CharacterCardId == characterCardId);
      if (candidate == null)
      {
        gameCandidates.RemoveRange(candidates);
        Add(new GameCandidate()
        {
          GameId = gameId,
          UserId = userId,
          CharacterCardId = characterCardId,
          DateTimeAdded = DateTime.Now
        });
      }
      else
      {
        gameCandidates.RemoveRange(candidates.Where(x => x.Id != candidate.Id));
        candidate.CharacterCardId = characterCardId;
        candidate.DateTimeAdded = DateTime.Now;
      }
    }
    //reload cache
  }
}