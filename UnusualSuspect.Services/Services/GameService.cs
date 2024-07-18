using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;
using ElmahCore;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Contracts;
using Microsoft.Extensions.Options;
using UnusualSuspect.Entities.Dtos;
using UnusualSuspect.ViewModels.Game;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.Logging;
using UnusualSuspect.Services.Timer;
using UnusualSuspect.ApiViewModels.SignalCommandsData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;

namespace UnusualSuspect.Services.Services;

public sealed class GameService(IUnitOfWork uow,
  IGameRepository gameRepository,
  IParticipateRepository participateRepository,
  ICharacterCardGameRepository characterCardGameRepository,
  INotificationService notificationService,
  IMemoryCacheService memoryCacheService,
  IQuestionGameRepository questionGameRepository,
  IPreGameGroupRepository preGameGroupRepository,
  IJoinedPreGameRepository joinedPreGameRepository,
  IGameCandidateRepository gameCandidateRepository,
  IQuestionService questionService,
  IScoreService scoreService,
  ITimerManagementService timerManagementService,
  IOptionsSnapshot<ProjectSetting> setting,
  ILogger<GameService> logger) : IGameService
{
  public async Task<UnusualSuspectServiceResult<Game>> GetCurrentGameAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameAsync(userId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<Game>(new UnusualSuspectErrorResult(LogicErrorCode.UserIsNotInActiveGame));
    return new UnusualSuspectServiceResult<Game>(game);
  }
  public async Task<UnusualSuspectServiceResult<Game>> GetCurrentGameWithDetailsAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameWithDetailsAsync(userId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<Game>(new UnusualSuspectErrorResult(LogicErrorCode.UserIsNotInActiveGame));
    return new UnusualSuspectServiceResult<Game>(game);
  }

  public async Task<UnusualSuspectServiceResult<GameGetResponse>> GetGameResponseAsync(int userId, int? gameId = null,
    CancellationToken cancellationToken = default)
  {
    Game? game;
    if (gameId.HasValue)
    {
      if (await participateRepository.GetParticipantRoleAsync(gameId.Value, userId, cancellationToken) == null)
        return new UnusualSuspectServiceResult<GameGetResponse>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
      game = await gameRepository.GetGameWithDetailsAsync(gameId.Value, cancellationToken);
      if (game == null)
        return new UnusualSuspectServiceResult<GameGetResponse>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    }
    else
    {
      var result = await GetCurrentGameWithDetailsAsync(userId, cancellationToken);
      if (!result.Success)
        return new UnusualSuspectServiceResult<GameGetResponse>(result.Errors);
      game = result.Result;
    }
    return new UnusualSuspectServiceResult<GameGetResponse>(new GameGetResponse(
      game.ToGameBaseDto(), game.ToGameFlowDto(),
      await memoryCacheService.GetSignalRGroupOnlineUsers(game.Id.ToString()), game.GameStatusId,
      game.ToPrivateInfoDto(userId), game.CachedTime.ToString(), setting.Value.GameSetting.TimeToTalkInSeconds));
  }


  public async Task<UnusualSuspectServiceResult<bool>> LeaveCurrentGameAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameAsync(userId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserIsNotInActiveGame));
    return await LeaveGameAsync(game.Id, userId, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<bool>> LeaveGameAsync(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    int participantCount = await participateRepository.GetParticipantCountAsync(gameId, cancellationToken);
    if (participantCount <= 0)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GameHasNoParticipants));
    if (participantCount <= 4)
      return await FinishGameAsync(gameId, null, cancellationToken);
    Participate? participate = (await participateRepository.GetActiveParticipations(userId, cancellationToken))
      .FirstOrDefault(x => x.GameId == gameId);
    if (participate == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
    participate.IsActive = false;

    RoleCardEnum userRole = (RoleCardEnum)participate.RoleCardId;
    switch (userRole)
    {
      case RoleCardEnum.Witness:
      case RoleCardEnum.Detective:
      case RoleCardEnum.Accomplice:
        //do nothing
        break;
      case RoleCardEnum.MainDetective:
        UnusualSuspectServiceResult<bool> result = await ReplaceRoleByDetective(gameId, userRole, userId);
        if (!result.Success)
          return result;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }

    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.UserLeftTheGame, userId);
    memoryCacheService.ClearGameWithDetails(gameId);
    return new UnusualSuspectServiceResult<bool>(true);
  }
  private async Task<UnusualSuspectServiceResult<bool>> ReplaceRoleByDetective(int gameId, RoleCardEnum leftUserRole, int leftUserId)
  {
    List<Participate> participants = (await participateRepository
      .GetGameActiveParticipantsAsync(gameId, RoleCardEnum.Detective))
      .Where(x => x.UserId != leftUserId).ToList();
    if (!participants.Any())
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.NoDetectiveInGameToReplaceUser));
    int random = new Random().Next(0, participants.Count - 1);
    participants[random].RoleCardId = (short)leftUserRole;
    return new UnusualSuspectServiceResult<bool>(true);
  }

  private async Task<UnusualSuspectServiceResult<bool>> FinishGameAsync(int gameId, GameStatusEnum? finalGameStatus = null,
    CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetByIdAsync(gameId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    if (finalGameStatus.HasValue)
    {
      gameRepository.SetGameStatus(game, finalGameStatus.Value);
      if (finalGameStatus.Value == GameStatusEnum.FinishedAndLostTheGame ||
          finalGameStatus.Value == GameStatusEnum.FinishedAndWonTheGame)
      {
        List<Participate> pars =
          await participateRepository.GetGameActiveParticipantsAsync(gameId, null, cancellationToken);
        await scoreService.SetGameFinishedScoresAsync(gameId,
          finalGameStatus.Value == GameStatusEnum.FinishedAndWonTheGame, pars, cancellationToken);
      }
    }
    game.FinishedTime = DateTime.Now;
    List<int> preGameGroupIds = await preGameGroupRepository.ResetGroupsStatusAfterFinishingTheGameAsync(gameId, cancellationToken);
    await joinedPreGameRepository.ResetJoinedPreGameAfterFinishingTheGameAsync(preGameGroupIds, cancellationToken);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.GameFinished, gameId);
    memoryCacheService.ClearGameWithDetails(gameId);
    return new UnusualSuspectServiceResult<bool>(true);
  }


  public async Task<UnusualSuspectServiceResult<bool?>> ChooseCardAndGetWinCondition(int gameId, int characterCardId, int userId, CancellationToken cancellationToken = default)
  {
    TimerManagementService.OnGameTimerStop(gameId);
    if (userId != -1)//for auto choose
    {
      bool hasAccess = await HasSpecificRoleInTheGame(gameId, userId, RoleCardEnum.MainDetective, cancellationToken);
      if (!hasAccess)
        return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.AccessIsDenied));
    }
    var cards = await characterCardGameRepository.GetAllGameCharacterCardsAsync(gameId, cancellationToken);
    if (cards.All(x => x.CharacterCardId != characterCardId))
      return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.CharacterCardIdNotFoundInTheGame));
    var card = cards.FirstOrDefault(x => x.CharacterCardId == characterCardId && x.IsActive);
    if (card == null)
      return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.CharacterCardIsNotActiveInTheGame));
    UnusualSuspectServiceResult<bool?> result;
    if (card.IsMurderer)
    {
      await FinishGameAsync(gameId, GameStatusEnum.FinishedAndLostTheGame, cancellationToken);
      result = new UnusualSuspectServiceResult<bool?>(false);
    }
    else
    {
      card.IsActive = false;
      characterCardGameRepository.Update(card);
      if (!cards.Any(x => x.IsActive && x.IsMurderer))
      {
        ElmahExtensions.RaiseError(new Exception("Game not have Active murderer. gameId: " + gameId));
        return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.NoActiveMurdererFoundInGame));
      }
      if (!cards.Any(x => x.IsActive && !x.IsMurderer))
      {
        await FinishGameAsync(gameId, GameStatusEnum.FinishedAndWonTheGame, cancellationToken);
        result = new UnusualSuspectServiceResult<bool?>(true);
      }
      else
      {
        if (await SetAnswerIfNoWitnessInGame(gameId, cancellationToken))
        {
          Game? game = await gameRepository.GetByIdAsync(gameId, cancellationToken);
          if (game == null)
            return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
          await GoToTalkingStatus(game);
        }
        else
        {
          await gameRepository.SetGameStatusAsync(gameId, GameStatusEnum.WaitingForWitnessToAnswer, cancellationToken);
          timerManagementService.OnGameTimerStart(gameId, GameTimerEnum.AutoAnswerQuestion);
        }
        result = new UnusualSuspectServiceResult<bool?>((bool?)null);
      }
    }
    await gameCandidateRepository.DeleteAllGameCandidatesAsync(gameId, cancellationToken);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.NewCardWasChosen, characterCardId);
    memoryCacheService.ClearGameWithDetails(gameId);
    return result;
  }

  private async Task<bool> SetAnswerIfNoWitnessInGame(int gameId, CancellationToken cancellationToken = default)
  {
    if ((await participateRepository.GetGameActiveParticipantsAsync(gameId, RoleCardEnum.Witness, cancellationToken)).Any())
      return false;
    Game? game = await gameRepository.GetByIdAsync(gameId, cancellationToken);
    if (game == null)
      return false;
    Game? gameCached = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken);
    int turn = game.CharacterCardGames.Count(x => !x.IsActive) + 1;
    if (turn > 11)
    {
      ElmahExtensions.RaiseError(new Exception("invalid turn in method: SetAnswerIfNoWitnessInGame - " + turn));
      return false;
    }
    if (gameCached == null || !gameCached.CharacterCardGames.Any(x => x.IsActive && x.IsMurderer) &&
        gameCached.QuestionGames.All(x => x.Turn != turn))
      return false;
    short cardId = gameCached.CharacterCardGames.First(x => x.IsActive && x.IsMurderer).CharacterCardId;
    short questionId = gameCached.QuestionGames.First(x => x.Turn == turn).QuestionId;
    var result = await questionService.GetDefaultAnswer(
      cardId, questionId, cancellationToken);
    if (!result.Success)
    {
      logger.LogEvent(SystemEventType.NoDefaultAnswerAvailable, gameId,
        $"CharacterCardId: {cardId} and questionId: {questionId}", logLevel: LogLevel.Critical);
      return false;
    }
    game.WitnessLastAnswer = result.Result;
    return true;
  }

  public IQueryable<Game> GetAllActiveGamesWithGameType()
  {
    return gameRepository.GetAllActiveGamesWithGameType();
  }

  public async Task<bool> StartGameIfAllUsersOnline(int gameId, List<int> userIds)
  {
    Game? game = await gameRepository.GetByIdAsync(gameId);
    if (game == null || game.GameStatusId != (short)GameStatusEnum.WaitingForPlayers)
      return false;
    bool hasOfflineUser = await participateRepository.IsGameHasOtherActiveParticipantsAsync(gameId, userIds);
    if (hasOfflineUser)
      return false;
    if (await SetAnswerIfNoWitnessInGame(gameId))
      return await GoToTalkingStatus(game);
    timerManagementService.OnGameTimerStart(gameId, GameTimerEnum.AutoAnswerQuestion);
    return await gameRepository.SetGameStatusAsync(gameId, GameStatusEnum.WaitingForWitnessToAnswer);
  }

  public async Task<bool> GoToTalkingStatus(int gameId)
  {
    Game? game = await gameRepository.GetByIdAsync(gameId);
    if (game == null)
      return false;
    return await GoToTalkingStatus(game);
  }
  public async Task<bool> GoToTalkingStatus(Game game)
  {
    var result = await StartTurnOfPlayAsync(game.Id);
    if (!result.Success)
      return false;
    gameRepository.SetGameStatus(game, GameStatusEnum.Talking);
    game.CurrentUserTurnStartedTime = DateTime.Parse(result.Result.TalkingTurnStartedTimeString);
    game.TalkingTurnStartedTime = DateTime.Parse(result.Result.TalkingTurnStartedTimeString);
    game.OrderOfParticipationTalkBeginner = result.Result.OrderOfParticipationTalkBeginner;
    game.OrderOfParticipationTurnToTalk = result.Result.OrderOfParticipationTurnToTalk;
    return true;
  }

  public async Task<UnusualSuspectServiceResult<GameDetailsViewModel>> GetDetailByIdAsync(int gameId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken, true);
    if (game == null)
      return new UnusualSuspectServiceResult<GameDetailsViewModel>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    GameDetailsViewModel model = new GameDetailsViewModel()
    {
      GameGetResponse = new GameGetResponse(
        game.ToGameBaseDto(), game.ToGameFlowDto(),
        await memoryCacheService.GetSignalRGroupOnlineUsers(game.Id.ToString()), game.GameStatusId, null, game.CachedTime.ToString(), setting.Value.GameSetting.TimeToTalkInSeconds),
      FinishedTime = game.FinishedTime,
      CreateTime = game.CreateTime,
      GameStatusTitle = ((GameStatusEnum)game.GameStatusId).ToString()
    };
    return new UnusualSuspectServiceResult<GameDetailsViewModel>(model);
  }

  public async Task<UnusualSuspectServiceResult<bool>> SetWitnessAnswer(int gameId, bool witnessAnswer,
    short questionId, int? userId, CancellationToken cancellationToken = default)
  {
    TimerManagementService.OnGameTimerStop(gameId);
    if (userId.HasValue)
    {
      bool hasAccess = await HasSpecificRoleInTheGame(gameId, userId.Value, RoleCardEnum.Witness, cancellationToken);
      if (!hasAccess)
        return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.AccessIsDenied));
    }
    Game? game = await gameRepository.GetByIdAsync(gameId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    if (game.GameStatusId != (short)GameStatusEnum.WaitingForWitnessToAnswer)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GameIsNotInWaitingForWitnessToAnswerStatus));
    QuestionGame? questionGame = await questionGameRepository.GetQuestionGameByQuestionAndGame(gameId, questionId);
    if (questionGame == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.NoGameQuestionWithThisGameIdAndQuestionId));
    questionGame.AnswerUserId = userId;
    questionGame.UserAnswer = witnessAnswer;
    game.WitnessLastAnswer = witnessAnswer;
    await GoToTalkingStatus(game);
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async Task<bool> IsGameParticipantAsync(int userId, int gameId, CancellationToken cancellationToken = default)
  {
    return await participateRepository.IsGameParticipantAsync(gameId, userId, cancellationToken);
  }

  private async Task<bool> HasSpecificRoleInTheGame(int gameId, int userId, RoleCardEnum roleCard, CancellationToken cancellationToken = default)
  {
    var role = await participateRepository.GetParticipantRoleAsync(gameId, userId, cancellationToken);
    if (role == null)
      return false;
    return role.Value == roleCard;
  }

  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return await uow.SaveChangesAsync(cancellationToken);
  }

  public async Task<UnusualSuspectServiceResult<GameStatisticsDto>> GetGameStatisticsAsync(int userId, CancellationToken cancellationToken = default)
  {
    GameStatisticsDto? gameStatistics = await gameRepository.GetGameStatisticsAsync(userId, cancellationToken);
    if (gameStatistics == null)
      return new UnusualSuspectServiceResult<GameStatisticsDto>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserId));
    return new UnusualSuspectServiceResult<GameStatisticsDto>(gameStatistics);
  }

  public async Task<UnusualSuspectServiceResult<FinishedResponse>> GetFinishedResponseAsync(
    int userId, int? gameId, CancellationToken cancellationToken = default)
  {
    Game? game;
    if (gameId.HasValue)
    {
      if (await participateRepository.GetParticipantRoleAsync(gameId.Value, userId, cancellationToken) == null)
        return new UnusualSuspectServiceResult<FinishedResponse>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
      game = await gameRepository.GetGameWithDetailsAsync(gameId.Value, cancellationToken);
      if (game == null)
        return new UnusualSuspectServiceResult<FinishedResponse>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    }
    else
    {
      var result = await GetCurrentGameWithDetailsAsync(userId, cancellationToken);
      if (!result.Success)
        return new UnusualSuspectServiceResult<FinishedResponse>(result.Errors);
      game = result.Result;
    }
    if (game.GameStatusId != (short)GameStatusEnum.FinishedAndLostTheGame &&
       game.GameStatusId != (short)GameStatusEnum.FinishedAndWonTheGame)
      return new UnusualSuspectServiceResult<FinishedResponse>(new UnusualSuspectErrorResult(LogicErrorCode.GameNotFinished));
    return new UnusualSuspectServiceResult<FinishedResponse>(game.ToFinishedResponse());
  }
  public async Task<UnusualSuspectServiceResult<TurnOfPlayTalkingState>> StartTurnOfPlayAsync(int gameId)
  {
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId);
    if (game == null)
      return new UnusualSuspectServiceResult<TurnOfPlayTalkingState>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    short starter = GetStarterOrderOfParticipation(game.Participates);
    TurnOfPlayGetResponse model = new TurnOfPlayGetResponse(new TurnOfPlayTalkingState()
    {
      TalkingTurnStartedTimeString = DateTime.Now.ToString(),
      OrderOfParticipationTalkBeginner = starter,
      OrderOfParticipationTurnToTalk = starter
    }, DateTime.Now.ToString());
    await memoryCacheService.ResetTurnOfPlay(game.Id, model.TurnOfPlayTalkingState!, DateTime.Now);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.PlayersStartToTalk, starter);
    timerManagementService.OnGameTimerStart(game.Participates.First(x => x.OrderOfParticipation == starter).UserId, gameId,
       GameTimerEnum.UserTurnFinished);
    return new UnusualSuspectServiceResult<TurnOfPlayTalkingState>(model.TurnOfPlayTalkingState!);
  }

  public async Task<UnusualSuspectServiceResult<bool>> UserTurnFinishedAsync(int userId, int gameId,
    CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    TurnOfPlayTalkingState? model = game.ToTurnOfPlayTalkingState();
    if (model == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GameIsNotInTalkingStatus));
    var partTalking = game.Participates.FirstOrDefault(x => x.UserId == userId);
    if (partTalking == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
    if (model.OrderOfParticipationTurnToTalk != partTalking.OrderOfParticipation)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserCallingFinishTalkIsNotTalking));
    var next = GetNextUserOrderOfParticipation(game.Id, game.Participates,
      model.OrderOfParticipationTurnToTalk);
    if (next.OrderOfParticipation == model.OrderOfParticipationTalkBeginner)
    {
      await notificationService.SendSignalToGameGroup(gameId, SignalCommands.EndOfTalking, gameId);
      await gameRepository.SetGameStatusAsync(gameId, GameStatusEnum.WaitingForMainDetectiveToChoose, cancellationToken);
      await gameRepository.SaveChangesAsync(cancellationToken);
      memoryCacheService.ClearGameWithDetails(gameId);
      timerManagementService.OnGameTimerStart(gameId, GameTimerEnum.AutoChooseCard);
      return new UnusualSuspectServiceResult<bool>(false);
    }
    model.OrderOfParticipationTurnToTalk = next.OrderOfParticipation;
    DateTime currentUserTurnStartedTime = DateTime.Now;
    await gameRepository.SetNewTurnToTalk(game.Id, model.OrderOfParticipationTurnToTalk,
      currentUserTurnStartedTime, cancellationToken);
    await gameRepository.SaveChangesAsync(cancellationToken);
    await memoryCacheService.ResetTurnOfPlay(gameId, model, currentUserTurnStartedTime, cancellationToken);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.PlayerTurnChange,
      model.OrderOfParticipationTurnToTalk);
    timerManagementService.OnGameTimerStart(next.UserId, gameId, GameTimerEnum.UserTurnFinished);
    return new UnusualSuspectServiceResult<bool>(true);
  }





  public async Task<UnusualSuspectServiceResult<TurnOfPlayGetResponse>> GetTurnOfPlayGetAsync(
    int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameWithDetailsAsync(userId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<TurnOfPlayGetResponse>(
        new UnusualSuspectErrorResult(LogicErrorCode.UserIsNotInActiveGame));
    var model = game.ToTurnOfPlayGetResponse();
    return new UnusualSuspectServiceResult<TurnOfPlayGetResponse>(model);
  }

  public async Task<UnusualSuspectServiceResult<bool>> ChangedCandidateCard(int userId, short? characterCardId, int gameId)
  {
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    var partTalking = game.Participates.FirstOrDefault(x => x.UserId == userId);
    if (partTalking == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GameIsNotInTalkingStatus));
    TurnOfPlayTalkingState? model = game.ToTurnOfPlayTalkingState();
    if (model == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
    var candidates = game.GameCandidates;
    var candid = candidates.FirstOrDefault(x => x.UserId == userId);
    if (candid != null)
    {
      var oldChoice = candid.CharacterCardId;
      if (characterCardId.HasValue)
      {
        if (characterCardId.Value == oldChoice)
          return new UnusualSuspectServiceResult<bool>(false);
        await gameCandidateRepository.ChangeUserCandidateInGameAsync(gameId, userId, characterCardId.Value);
      }
      else
      {
        await gameCandidateRepository.DeleteUserCandidatesInGameAsync(gameId, userId);
      }
    }
    else
    {
      if (!characterCardId.HasValue)
        return new UnusualSuspectServiceResult<bool>(false);
      gameCandidateRepository.Add(new GameCandidate()
      {
        CharacterCardId = characterCardId.Value,
        GameId = gameId,
        UserId = userId,
        DateTimeAdded = DateTime.Now
      });
    }
    await gameCandidateRepository.SaveChangesAsync();
    var newCandidates = await gameCandidateRepository.GetAllGameCandidatesAsync(gameId);
    await memoryCacheService.ResetGameCandidates(gameId, newCandidates);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.CandidateCardChange, new CandidateCardChangeViewModel()
    {
      UserId = userId,
      CharacterCardId = characterCardId
    });
    return new UnusualSuspectServiceResult<bool>(true);
  }

  private short GetStarterOrderOfParticipation(ICollection<Participate> gameParticipates)
  {
    int random = new Random().Next(0, gameParticipates.Count - 2);
    return gameParticipates
      .Where(x => x.RoleCardId != (short)RoleCardEnum.Witness)
      .ToList()[random]
      .OrderOfParticipation;
  }
  private Participate GetNextUserOrderOfParticipation(int gameId, ICollection<Participate> participates,
    short currentOrderOfParticipation)
  {
    Participate? next = participates.Where(x => x.RoleCardId != (short)RoleCardEnum.Witness && x.OrderOfParticipation > currentOrderOfParticipation)
      .MinBy(x => x.OrderOfParticipation);
    if (next != null)
      return next;
    next = participates.Where(x => x.RoleCardId != (short)RoleCardEnum.Witness).MinBy(x => x.OrderOfParticipation);
    if (next == null)
      throw new Exception("No participants found! gameId: " + gameId);
    return next;
  }

}