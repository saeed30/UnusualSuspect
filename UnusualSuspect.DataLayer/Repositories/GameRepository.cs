using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Dtos;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GameRepository(
    IUnitOfWork uow,
    ILogger<GameRepository> logger,
    IMemoryCacheService memoryCacheService,
    IOptionsSnapshot<ProjectSetting> setting,
    IDapperRepository dapperRepository)
  : EfRepository<Game>(uow, logger), IGameRepository
{
  private readonly DbSet<Game> games = uow.Set<Game>();

  public async Task<Game?> GetGameWithDetailsAsync(int id, CancellationToken cancellationToken = default, bool ignoreCache = false)
  {
    //if(setting.Value.IsTesting)
    //  ignoreCache = true;//saeed remove after test
    Game? game = null;
    if(!ignoreCache)
      game = await memoryCacheService.GetGameWithDetails(id, cancellationToken);
    if (game == null)
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

  public Task<GameStatisticsDto?> GetGameStatisticsAsync(int userId, CancellationToken cancellationToken = default)
  {
    return dapperRepository.QuerySingleAsync<GameStatisticsDto>($@"
select GamesPlayed = COUNT(1),
GamesWon = isnull(SUM(case when g.GameStatusId = {(short)GameStatusEnum.FinishedAndWonTheGame} then 1 else 0 end), 0),
GamesLost = isnull(SUM(case when g.GameStatusId = {(short)GameStatusEnum.FinishedAndLostTheGame} then 1 else 0 end), 0)
from Participate p join Game g on p.GameId = g.Id
where g.FinishedTime is not null and p.UserId = {userId}");
  }
}