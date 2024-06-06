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
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.ApiViewModels.SignalCommandsData;

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
      game.ToGameBaseDto(), game.ToGameFlowDto(setting.Value.GameSetting.TimeToTalkInSeconds),
      await memoryCacheService.GetSignalRGroupOnlineUsers(game.Id.ToString()), game.GameStatusId,
      game.ToPrivateInfoDto(userId)));
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
    TimerManagement.OnTimerStop(gameId);
    bool hasAccess = await HasSpecificRoleInTheGame(gameId, userId, RoleCardEnum.MainDetective, cancellationToken);
    if (!hasAccess)
      return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.AccessIsDenied));
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
          if (setting.Value.GameSetting.TimeToTalkInSeconds > 0)
            TimerManagement.OnTimerStart(-1, gameId, new TimeSpan(0, 0, setting.Value.GameSetting.TimeToTalkInSeconds), AutoAnswerQuestion);
        }
        result = new UnusualSuspectServiceResult<bool?>((bool?)null);
      }
    }
    await gameCandidateRepository.DeleteAllGameCandidatesAsync(gameId, cancellationToken);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.NewCardWasChosen);
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
      logger.LogWarning(
        "Default answer was requested for gameId: {gameId} but no default answer was present for CharacterCardId: {cardId} and questionId: {questionId}",
        gameId, cardId, questionId);
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
    if (setting.Value.GameSetting.TimeToTalkInSeconds > 0)
      TimerManagement.OnTimerStart(-1, gameId, new TimeSpan(0, 0, setting.Value.GameSetting.TimeToTalkInSeconds), AutoAnswerQuestion);
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
    game.CurrentUserTurnStartedTime = result.Result.CurrentUserTurnStartedTime;
    game.TalkingTurnStartedTime = result.Result.TalkingTurnStartedTime;
    game.OrderOfParticipationTalkBeginner = result.Result.OrderOfParticipationTalkBeginner;
    game.OrderOfParticipationTurnToTalk = result.Result.OrderOfParticipationTurnToTalk;
    return true;
  }
  public async void AutoChooseCard(int userId, int gameId)
  {
    try
    {
      var participants = (await participateRepository.GetGameActiveParticipantsAsync(gameId, RoleCardEnum.MainDetective)).FirstOrDefault();
      if (participants == null)
      {
        logger.LogError("MainDetective not found in game ({gameId})", gameId);
        return;
      }

      var game = await GetDetailByIdAsync(gameId);
      if (!game.Success)
      {
        logger.LogError("Game not found with id ({gameId}). error: {error}", gameId, game.MainError.ToString());
        return;
      }

      List<short> activeCharacters = game.Result.GameGetResponse.GameFlowDto.ActiveCharacterIds;
      List<CandidateCardDto> candidate = game.Result.GameGetResponse.GameFlowDto.CandidateCard
        .Where(x => activeCharacters.Contains((short)x.CharacterCardId)).ToList();
      int characterId;
      if (candidate == null || !candidate.Any())
      {
        logger.LogWarning("No candidate were found for auto choose card for game ({gameId})", gameId);
        characterId = new Random().Next(0, activeCharacters.Count - 1); ;
        await ChooseCardAndGetWinCondition(gameId, activeCharacters[characterId], userId);
        return;
      }
      var result = candidate.GroupBy(x => x.CharacterCardId)
        .Select(x => new { CharacterCardId = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).ToList();
      if (result.Count < 2 || result[0].Count == result[1].Count)
      {
        logger.LogWarning(
          "No candidate with most vote were found for auto choose card for game ({gameId}). number of candidates: {CandidateCount}",
          gameId, candidate.Count);
        characterId = new Random().Next(0, activeCharacters.Count - 1);
        await ChooseCardAndGetWinCondition(gameId, activeCharacters[characterId], userId);
        return;
      }
      await ChooseCardAndGetWinCondition(gameId, result[0].CharacterCardId, userId);
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
      throw;
    }

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
        game.ToGameBaseDto(), game.ToGameFlowDto(setting.Value.GameSetting.TimeToTalkInSeconds),
        await memoryCacheService.GetSignalRGroupOnlineUsers(game.Id.ToString()), game.GameStatusId, null),
      FinishedTime = game.FinishedTime,
      CreateTime = game.CreateTime,
      GameStatusTitle = ((GameStatusEnum)game.GameStatusId).ToString()
    };
    return new UnusualSuspectServiceResult<GameDetailsViewModel>(model);
  }

  public async Task<UnusualSuspectServiceResult<bool>> SetWitnessAnswer(int gameId, bool witnessAnswer,
    short questionId, int userId, CancellationToken cancellationToken = default)
  {
    TimerManagement.OnTimerStop(gameId);
    bool hasAccess = await HasSpecificRoleInTheGame(gameId, userId, RoleCardEnum.Witness, cancellationToken);
    if (!hasAccess)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.AccessIsDenied));
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
      CurrentUserTurnStartedTime = DateTime.Now,
      TalkingTurnStartedTime = DateTime.Now,
      OrderOfParticipationTalkBeginner = starter,
      OrderOfParticipationTurnToTalk = starter
    });
    await memoryCacheService.ResetTurnOfPlay(game.Id, model.TurnOfPlayTalkingState!);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.PlayersStartToTalk, starter);
    if (setting.Value.GameSetting.TimeToTalkInSeconds > 0)
      TimerManagement.OnTimerStart(game.Participates.First(x => x.OrderOfParticipation == starter).UserId, gameId,
         new TimeSpan(0, 0, setting.Value.GameSetting.TimeToTalkInSeconds), UserTurnFinished);
    return new UnusualSuspectServiceResult<TurnOfPlayTalkingState>(model.TurnOfPlayTalkingState!);
  }

  public async Task<UnusualSuspectServiceResult<bool>> UserTurnFinishedAsync(int userId, int gameId,
    CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    TurnOfPlayTalkingState? model = game.ToTurnOfPlayTalkingState(setting.Value.GameSetting.TimeToTalkInSeconds);
    if (model == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GameIsNotInTalkingStatus));
    var partTalking = game.Participates.FirstOrDefault(x => x.UserId == userId);
    if (partTalking == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
    if (model.OrderOfParticipationTurnToTalk != partTalking.OrderOfParticipation)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserCallingFinishTalkIsNotTalking));
    var next = GetNextUserOrderOfParticipation(game.Participates,
      model.OrderOfParticipationTurnToTalk);
    if (next.OrderOfParticipation == model.OrderOfParticipationTalkBeginner)
    {
      await notificationService.SendSignalToGameGroup(gameId, SignalCommands.EndOfTalking, gameId);
      await gameRepository.SetGameStatusAsync(gameId, GameStatusEnum.WaitingForMainDetectiveToChoose, cancellationToken);
      await gameRepository.SaveChangesAsync(cancellationToken);
      memoryCacheService.ClearGameWithDetails(gameId);
      if (setting.Value.GameSetting.TimeToTalkInSeconds > 0)
        TimerManagement.OnTimerStart(-1, gameId, new TimeSpan(0, 0, setting.Value.GameSetting.TimeToTalkInSeconds), AutoChooseCard);
      return new UnusualSuspectServiceResult<bool>(false);
    }
    model.OrderOfParticipationTurnToTalk = next.OrderOfParticipation;
    model.CurrentUserTurnStartedTime = DateTime.Now;
    await gameRepository.SetNewTurnToTalk(game.Id, model.OrderOfParticipationTurnToTalk,
      model.CurrentUserTurnStartedTime, cancellationToken);
    await gameRepository.SaveChangesAsync(cancellationToken);
    await memoryCacheService.ResetTurnOfPlay(gameId, model, cancellationToken);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.PlayerTurnChange,
      model.OrderOfParticipationTurnToTalk);
    if (setting.Value.GameSetting.TimeToTalkInSeconds > 0)
      TimerManagement.OnTimerStart(next.UserId, gameId, new TimeSpan(0, 0, setting.Value.GameSetting.TimeToTalkInSeconds), UserTurnFinished);
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async void AutoAnswerQuestion(int userId, int gameId)
  {

  }
  public async void UserTurnFinished(int userId, int gameId)
  {
    try
    {
      await UserTurnFinishedAsync(userId, gameId);
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
    }

  }
  public async Task<UnusualSuspectServiceResult<TurnOfPlayGetResponse>> GetTurnOfPlayGetAsync(
    int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameWithDetailsAsync(userId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<TurnOfPlayGetResponse>(
        new UnusualSuspectErrorResult(LogicErrorCode.UserIsNotInActiveGame));
    var model = game.ToTurnOfPlayGetResponse(setting.Value.GameSetting.TimeToTalkInSeconds);
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
    TurnOfPlayTalkingState? model = game.ToTurnOfPlayTalkingState(setting.Value.GameSetting.TimeToTalkInSeconds);
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
  private Participate GetNextUserOrderOfParticipation(ICollection<Participate> participates,
    short currentOrderOfParticipation)
  {
    Participate? next = participates.Where(x => x.RoleCardId != (short)RoleCardEnum.Witness && x.OrderOfParticipation > currentOrderOfParticipation)
      .MinBy(x => x.OrderOfParticipation);
    if (next != null)
      return next;
    next = participates.Where(x => x.RoleCardId != (short)RoleCardEnum.Witness).MinBy(x => x.OrderOfParticipation);
    if (next == null)
    {
      logger.LogCritical("No participants found!");
      throw new Exception("No participants found!");
    }
    return next;
  }

}