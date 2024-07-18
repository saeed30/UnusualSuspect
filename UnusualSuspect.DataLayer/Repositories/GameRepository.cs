using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Dtos;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class GameRepository(
    IUnitOfWork uow,
    ILogger<GameRepository> logger,
    IMemoryCacheService memoryCacheService,
    //IOptionsSnapshot<ProjectSetting> setting,
    IDapperRepository dapperRepository)
  : EfRepository<Game>(uow, logger), IGameRepository
{
  public async Task<Game?> GetGameWithDetailsAsync(int id, CancellationToken cancellationToken = default, bool ignoreCache = false)
  {
    Game? game = null;
    if (!ignoreCache)
      game = await memoryCacheService.GetGameWithDetails(id, cancellationToken);
    if (game == null)
    {
      game = await BaseEntity.AsNoTrackingWithIdentityResolution().AsSplitQuery()
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
    if (game.GameStatusId == (short)GameStatusEnum.Talking &&
      (!game.OrderOfParticipationTurnToTalk.HasValue ||
        !game.OrderOfParticipationTalkBeginner.HasValue ||
        !game.TalkingTurnStartedTime.HasValue ||
        !game.CurrentUserTurnStartedTime.HasValue))
      logger.LogEvent(SystemEventType.InvalidStatusDataOnTalking, game.Id, logLevel: LogLevel.Warning);

    if (game.GameCandidates.Any())
    {
      foreach (var gameCandidate in game.GameCandidates)
      {
        if (game.GameCandidates.Any(x => x.Id != gameCandidate.Id && x.UserId == gameCandidate.UserId))
          logger.LogEvent(SystemEventType.GameWithUserWithMultipleCandidates, game.Id, gameCandidate.UserId.ToString(), logLevel: LogLevel.Warning);
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
        game.CurrentUserTurnStartedTime = DateTime.Now;
        break;
      case GameStatusEnum.WaitingForMainDetectiveToChoose:
        game.CurrentUserTurnStartedTime = DateTime.Now;
        break;
    }
    return true;
  }

  public IQueryable<Game> GetAllActiveGamesWithGameType()
  {
    return BaseEntity
      .Include(x => x.GameType)
      .Where(x => x.FinishedTime == null);
  }

  public async Task<Game?> GetUserCurrentGameAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await BaseEntity.FirstOrDefaultAsync(
      x => x.FinishedTime == null &&
           x.Participates.Any(p => p.UserId == userId), cancellationToken);
    return game;
  }
  public async Task<Game?> GetUserCurrentGameWithDetailsAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await BaseEntity.FirstOrDefaultAsync(
      x => x.FinishedTime == null &&
           x.Participates.Any(p => p.UserId == userId && p.IsActive), cancellationToken);
    if (game == null)
      return null;
    return await GetGameWithDetailsAsync(game.Id, cancellationToken);
  }

  public async Task<bool> UserIsInActiveGameAsync(int userId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.AnyAsync(x =>
      x.FinishedTime == null && x.Participates.Any(p => p.UserId == userId && p.IsActive), cancellationToken);
  }

  public async Task SetNewTurnToTalk(int gameId, short orderOfParticipationTurnToTalk, DateTime currentUserTurnStartedTime, CancellationToken cancellationToken = default)
  {
    Game? game = await GetByIdAsync(gameId, cancellationToken);
    if (game == null)
    {
      logger.LogEvent(SystemEventType.InvalidGameIdInSetNewTurnToTalk, gameId, logLevel: LogLevel.Critical);
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