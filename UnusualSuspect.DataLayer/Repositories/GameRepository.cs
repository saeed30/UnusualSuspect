using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GameRepository(IUnitOfWork uow, ILogger<GameRepository> logger, IMemoryCacheService memoryCacheService)
  : EfRepository<Game>(uow, logger), IGameRepository
{
  private readonly DbSet<Game> games = uow.Set<Game>();

  public async Task<Game?> GetGameWithDetailsAsync(int id, CancellationToken cancellationToken = default)
  {
	  bool isCacheActive = false;
    Game? game = await memoryCacheService.GetGameWithDetails(id, cancellationToken);
    if (game == null || !isCacheActive)
    {
      game = await games.AsNoTrackingWithIdentityResolution().AsSplitQuery()
        .Include(x => x.CharacterCardGames)
        .ThenInclude(x => x.CharacterCard)
        .Include(x => x.GameType)
        .Include(c => c.Participates)
        .ThenInclude(c => c.ApplicationUser)
        .ThenInclude(c => c.Document)
        .Include(x => x.GameCandidates)
        .Include(x => x.QuestionGames)
        .ThenInclude(x => x.Question)
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
      if (game == null)
        return null;
      ValidateGameData(game);
      memoryCacheService.SetGameWithDetails(game);
    }
    return game;
  }

  private void ValidateGameData(Game game)
  {
    if (game.GameStatusId != (short)GameStatusEnum.Talking &&
      (!game.OrderOfParticipationTurnToTalk.HasValue ||
        !game.OrderOfParticipationTalkBeginner.HasValue ||
        !game.TalkingTurnStartedTime.HasValue ||
        !game.CurrentUserTurnStartedTime.HasValue))
      logger.LogCritical("Invalid status data on Talking. gameId: {gameId}", game.Id);
    if (game.GameCandidates.Any())
    {
      foreach (var gameCandidate in game.GameCandidates)
      {
        if (game.GameCandidates.Any(x => x.Id != gameCandidate.Id && x.UserId == gameCandidate.UserId))
          logger.LogCritical("Game has multiple candidate for one user. gameId: {gameId} - userId: {userId}", game.Id, gameCandidate.UserId);
      }
    }
  }

  public async Task<bool> SetGameStatusAsync(int id, GameStatusEnum gameStatus,
    CancellationToken cancellationToken = default)
  {
    var game = await GetByIdAsync(id, cancellationToken);
    if (game == null)
      return false;
    return SetGameStatus(game, gameStatus);
  }
  public bool SetGameStatus(Game game, GameStatusEnum gameStatus)
  {
    game.GameStatusId = (short)gameStatus;
    if (game.GameStatusId != (short)GameStatusEnum.Talking)
    {
      game.OrderOfParticipationTurnToTalk = null;
      game.OrderOfParticipationTalkBeginner = null;
      game.TalkingTurnStartedTime = null;
      game.CurrentUserTurnStartedTime = null;
    }
    switch (gameStatus)
    {
      case GameStatusEnum.WaitingForWitnessToAnswer:
        game.WitnessLastAnswer = null;
        break;
    }
    return true;
  }

  public IQueryable<Game> GetAllActiveGamesWithGameType()
  {
    return games
      .Include(x => x.GameType)
      .Where(x => x.FinishedTime == null);
  }

  public async Task<Game?> GetUserCurrentGameAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await games.FirstOrDefaultAsync(
      x => x.FinishedTime == null &&
           x.Participates.Any(p => p.UserId == userId), cancellationToken);
    return game;
  }
  public async Task<Game?> GetUserCurrentGameWithDetailsAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await games.FirstOrDefaultAsync(
      x => x.FinishedTime == null &&
           x.Participates.Any(p => p.UserId == userId && p.IsActive), cancellationToken);
    if (game == null)
      return null;
    return await GetGameWithDetailsAsync(game.Id, cancellationToken);
  }

  public async Task<bool> UserIsInActiveGameAsync(int userId, CancellationToken cancellationToken = default)
  {
    return await games.AnyAsync(x =>
      x.FinishedTime == null && x.Participates.Any(p => p.UserId == userId && p.IsActive), cancellationToken);
  }

  public async Task SetNewTurnToTalk(int gameId, short orderOfParticipationTurnToTalk, DateTime currentUserTurnStartedTime, CancellationToken cancellationToken = default)
  {
	  Game? game = await GetByIdAsync(gameId, cancellationToken);
	  if (game == null)
	  {
      logger.LogError(new Exception("Invalid gameId"), "Invalid gameId");
		  return;
	  }
	  game.OrderOfParticipationTurnToTalk = orderOfParticipationTurnToTalk;
	  game.CurrentUserTurnStartedTime = currentUserTurnStartedTime;
  }
}