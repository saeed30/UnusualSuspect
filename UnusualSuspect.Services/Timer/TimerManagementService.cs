using ElmahCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.DataLayer.Repositories;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Timer;

public enum GameTimerEnum
{
  AutoChooseCard,
  AutoAnswerQuestion,
  UserTurnFinished,
  UserTurnFinishedForBot,
}
public enum UserTimerEnum
{
  OutOfGameTimeout,
}
public class TimerManagementService(
  IOptionsSnapshot<ProjectSetting> setting,
  IServiceScopeFactory scopeFactory) : ITimerManagementService
{
  private static Dictionary<int, System.Timers.Timer> _gameTimers = new();
  private static Dictionary<int, System.Timers.Timer> _userTimers = new();

  public void OnGameTimerStart(int gameId, GameTimerEnum gameTimerEnum)
  {
    OnGameTimerStart(-1, gameId, gameTimerEnum);
  }
  public void OnGameTimerStart(int userId, int gameId, GameTimerEnum gameTimerEnum)
  {
    if (setting.Value.GameSetting.TimeToTalkInSeconds <= 0)
      return;
    TimerManagementService.OnGameTimerStop(gameId);
    switch (gameTimerEnum)
    {
      case GameTimerEnum.AutoChooseCard:
        OnGameTimerStart(userId, gameId, TimeSpan.FromSeconds(setting.Value.GameSetting.TimeToTalkInSeconds), AutoChooseCardWithNewScope);
        break;
      case GameTimerEnum.AutoAnswerQuestion:
        OnGameTimerStart(userId, gameId, TimeSpan.FromSeconds(setting.Value.GameSetting.TimeToTalkInSeconds), AutoAnswerQuestionWithNewScope);
        break;
      case GameTimerEnum.UserTurnFinished:
        OnGameTimerStart(userId, gameId, TimeSpan.FromSeconds(setting.Value.GameSetting.TimeToTalkInSeconds), UserTurnFinishedWithNewScope);
        break;
      case GameTimerEnum.UserTurnFinishedForBot:
        Random r = new Random();
        int sec = r.Next(setting.Value.GameSetting.TimeToTalkInSeconds / 4, setting.Value.GameSetting.TimeToTalkInSeconds * 3 / 4);
        OnGameTimerStart(userId, gameId, TimeSpan.FromSeconds(sec), UserTurnFinishedWithNewScope);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(gameTimerEnum), gameTimerEnum, null);
    }
  }
  public void OnUserTimerStart(int userId, UserTimerEnum userTimerEnum)
  {
    if (setting.Value.GameSetting.TimeToTalkInSeconds <= 0)
      return;
    TimerManagementService.OnUserTimerStop(userId);
    switch (userTimerEnum)
    {
      case UserTimerEnum.OutOfGameTimeout:
        if (setting.Value.GameSetting.TimeToTalkInSeconds <= 0)
          return;
        OnUserTimerStart(userId, TimeSpan.FromSeconds(setting.Value.GameSetting.TimeToTalkInSeconds * 3), LeaveCurrentGameWithNewScope);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(userTimerEnum), userTimerEnum, null);
    }
  }
  public static void OnGameTimerStop(int gameId)
  {
    if (_gameTimers.TryGetValue(gameId, out System.Timers.Timer? timer))
    {
      timer.Stop();
      timer.Dispose();
      _gameTimers.Remove(gameId);
    }
  }
  public static void OnUserTimerStop(int userId)
  {
    if (_userTimers.TryGetValue(userId, out System.Timers.Timer? timer))
    {
      timer.Stop();
      timer.Dispose();
      _userTimers.Remove(userId);
    }
  }
  private static void OnGameTimerStart(int userId, int gameId, TimeSpan timeSpan, Action<int, int> eventHandler)
  {
    if (_gameTimers.TryGetValue(gameId, out System.Timers.Timer? timer))
      timer.Stop();
    timer = new System.Timers.Timer(timeSpan);
    timer.AutoReset = false;
    timer.Elapsed += (sender, e) => eventHandler(userId, gameId);
    timer.Start();
    _gameTimers[gameId] = timer;
  }


  private static void OnUserTimerStart(int userId, TimeSpan timeSpan, Action<int> eventHandler)
  {
    if (_userTimers.TryGetValue(userId, out System.Timers.Timer? timer))
      timer.Stop();
    timer = new System.Timers.Timer(timeSpan);
    timer.AutoReset = false;
    timer.Elapsed += (sender, e) => eventHandler(userId);
    timer.Start();
    _userTimers[userId] = timer;
  }

  private async void LeaveCurrentGameWithNewScope(int userId)
  {
    using var scope = scopeFactory.CreateScope();
    IOptionsSnapshot<ProjectSetting> setting = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ProjectSetting>>();
    ILogger<TimerManagementService> loggerNew = scope.ServiceProvider.GetRequiredService<ILogger<TimerManagementService>>();
    //if (setting.Value.IsTesting)
    //{
    //  loggerNew.LogEvent(SystemEventType.LeaveCurrentGameNotDoneWhenTesting, userId);
    //  return;
    //}

    IGameService gameServiceNew = scope.ServiceProvider.GetRequiredService<IGameService>();
    try
    {
      loggerNew.LogEvent(SystemEventType.LeaveCurrentGameStartedForUser, userId);
      var result = await gameServiceNew.LeaveCurrentGameAsync(userId);
      if (!result.Success)
        loggerNew.LogEvent(SystemEventType.LeaveCurrentGameUnsuccessful, userId);
      else if (!result.Result)
        loggerNew.LogEvent(SystemEventType.LeaveCurrentGameFailed, userId);
      else
      {
        await gameServiceNew.SaveChangesAsync();
        loggerNew.LogEvent(SystemEventType.LeaveCurrentGameDone, userId);
      }
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
    }
  }

  private async void UserTurnFinishedWithNewScope(int userId, int gameId)
  {
    using var scope = scopeFactory.CreateScope();

    IGameService gameServiceNew = scope.ServiceProvider.GetRequiredService<IGameService>();
    ILogger<TimerManagementService> loggerNew = scope.ServiceProvider.GetRequiredService<ILogger<TimerManagementService>>();
    IApplicationUserManager applicationUserManagerNew = scope.ServiceProvider.GetRequiredService<IApplicationUserManager>();
    IGameRepository gameRepositoryNew = scope.ServiceProvider.GetRequiredService<IGameRepository>();
    loggerNew.LogEvent(SystemEventType.UserTurnFinishedWithNewScopeStarted, gameId, userId.ToString());
    try
    {
      if (await applicationUserManagerNew.GetIsBot(userId))
      {
        Game? game = await gameRepositoryNew.GetGameWithDetailsAsync(gameId);
        if (game == null)
          throw new Exception("invalid gameId: " + gameId);
        var cards = game.CharacterCardGames.Where(x => x.IsActive).Select(x => x.CharacterCardId).ToList();
        if (cards.Any())
        {
          Random r = new Random();
          int index = r.Next(0, cards.Count);
          loggerNew.LogEvent(SystemEventType.AutoCandidateForBotUser, gameId, userId.ToString());
          await gameServiceNew.ChangedCandidateCard(userId, cards[index], gameId);
        }
      }
      await gameServiceNew.UserTurnFinishedAsync(userId, gameId);
      loggerNew.LogEvent(SystemEventType.UserTurnFinishedWithNewScopeFinished, gameId, userId.ToString());
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
    }
  }

  private async void AutoAnswerQuestionWithNewScope(int userId, int gameId)
  {
    using var scope = scopeFactory.CreateScope();

    IGameService gameServiceNew = scope.ServiceProvider.GetRequiredService<IGameService>();
    IQuestionService questionServiceNew = scope.ServiceProvider.GetRequiredService<IQuestionService>();
    ICharacterCardGameRepository characterCardGameRepositoryNew = scope.ServiceProvider.GetRequiredService<ICharacterCardGameRepository>();
    ILogger<TimerManagementService> loggerNew = scope.ServiceProvider.GetRequiredService<ILogger<TimerManagementService>>();
    IMemoryCacheService memoryCacheServiceNew = scope.ServiceProvider.GetRequiredService<IMemoryCacheService>();
    INotificationService notificationServiceNew = scope.ServiceProvider.GetRequiredService<INotificationService>();

    loggerNew.LogEvent(SystemEventType.AutoAnswerQuestionWithNewScopeStarted, gameId, userId.ToString());
    try
    {
      var game = (await gameServiceNew.GetDetailByIdAsync(gameId)).Result;
      if (game == null)
      {
        loggerNew.LogEvent(SystemEventType.AutoAnswerQuestionWithNewScopeInvalidGameId, gameId, userId.ToString());
        return;
      }

      short questionId = GetCurrentQuestion(game.GameGetResponse);
      CharacterCardGame? murderer = await characterCardGameRepositoryNew.GetMurderer(gameId);
      if (murderer == null)
      {
        loggerNew.LogEvent(SystemEventType.AutoAnswerQuestionWithNewScopeMurdererNotFound, gameId, userId.ToString());
        return;
      }
      var answer = await questionServiceNew.GetDefaultAnswer(murderer.CharacterCardId, questionId);
      if (!answer.Success)
      {
        loggerNew.LogEvent(SystemEventType.AutoAnswerQuestionWithNewScopeDefaultAnswerNotFound, gameId, userId.ToString());
        return;
      }
      await gameServiceNew.SetWitnessAnswer(gameId, answer.Result, questionId, null);
      await gameServiceNew.SaveChangesAsync();
      await notificationServiceNew.SendSignalToGameGroup(gameId, SignalCommands.WitnessAnswered, answer.Result);

      memoryCacheServiceNew.ClearGameWithDetails(gameId);
      loggerNew.LogEvent(SystemEventType.AutoAnswerQuestionWithNewScopeFinished, gameId, userId.ToString());
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
    }
  }
  private short GetCurrentQuestion(GameGetResponse gameGetResponse)
  {
    int turn = 12 - gameGetResponse.GameFlowDto.ActiveCharacterIds.Count;
    if (turn < 0)
      throw new Exception("Invalid active characterId count! " + gameGetResponse.GameFlowDto.ActiveCharacterIds.Count);
    if (turn >= 12)
      throw new Exception("No active card found! gameId: " + gameGetResponse.GameBaseDto.Id);
    return gameGetResponse.GameBaseDto.QuestionGameDtos[turn].QuestionId;
  }
  private async void AutoChooseCardWithNewScope(int userId, int gameId)
  {
    using var scope = scopeFactory.CreateScope();

    IGameService gameServiceNew = scope.ServiceProvider.GetRequiredService<IGameService>();
    IParticipateRepository participateRepositoryNew = scope.ServiceProvider.GetRequiredService<IParticipateRepository>();
    IMemoryCacheService memoryCacheServiceNew = scope.ServiceProvider.GetRequiredService<IMemoryCacheService>();
    INotificationService notificationServiceNew = scope.ServiceProvider.GetRequiredService<INotificationService>();
    ILogger<TimerManagementService> loggerNew = scope.ServiceProvider.GetRequiredService<ILogger<TimerManagementService>>();

    loggerNew.LogEvent(SystemEventType.AutoChooseCardWithNewScopeStarted, gameId, userId.ToString());
    try
    {
      var participants = (await participateRepositoryNew.GetGameActiveParticipantsAsync(gameId, RoleCardEnum.MainDetective)).FirstOrDefault();
      if (participants == null)
      {
        loggerNew.LogEvent(SystemEventType.AutoChooseCardWithNewScopeMainDetectiveNotFound, gameId, userId.ToString());
        return;
      }

      var game = await gameServiceNew.GetDetailByIdAsync(gameId);
      if (!game.Success)
      {
        loggerNew.LogEvent(SystemEventType.AutoChooseCardWithNewScopeErrorOnGettingGameInfo, gameId, game.MainError.ToString(), logLevel: LogLevel.Error);
        return;
      }

      List<short> activeCharacters = game.Result.GameGetResponse.GameFlowDto.ActiveCharacterIds;
      List<CandidateCardDto> candidate = game.Result.GameGetResponse.GameFlowDto.CandidateCard
        .Where(x => activeCharacters.Contains((short)x.CharacterCardId)).ToList();
      int characterId;
      UnusualSuspectServiceResult<bool?> chooseCardResult;
      if (candidate == null || !candidate.Any())
      {
        loggerNew.LogEvent(SystemEventType.AutoChooseCardWithNewScopeNoCandidate, gameId, userId.ToString());
        characterId = new Random().Next(0, activeCharacters.Count - 1); ;
        chooseCardResult = await gameServiceNew.ChooseCardAndGetWinCondition(gameId, activeCharacters[characterId], userId);
      }
      else
      {
        var result = candidate.GroupBy(x => x.CharacterCardId)
          .Select(x => new { CharacterCardId = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).ToList();
        if (result.Count >= 2 && result[0].Count == result[1].Count)
        {
          loggerNew.LogEvent(SystemEventType.AutoChooseCardWithNewScopeNoCandidateWithTopVote, gameId, userId.ToString());
          var charWithMax = result.Where(x => x.Count == result[0].Count).ToList();
          characterId = new Random().Next(0, charWithMax.Count - 1);
          chooseCardResult = await gameServiceNew.ChooseCardAndGetWinCondition(gameId, charWithMax[characterId].CharacterCardId, userId);
        }
        else
          chooseCardResult = await gameServiceNew.ChooseCardAndGetWinCondition(gameId, result[0].CharacterCardId, userId);
      }
      if (!chooseCardResult.Success)
        loggerNew.LogEvent(SystemEventType.AutoChooseCardWithNewScopeErrorOnChoose, gameId, userId.ToString(), null, LogLevel.Critical, null, chooseCardResult.MainError.ToString());
      await gameServiceNew.SaveChangesAsync();
      if (chooseCardResult.Result.HasValue)
        await notificationServiceNew.RemoveAllUsersFromGame(gameId);
      memoryCacheServiceNew.ClearGameWithDetails(gameId);
      loggerNew.LogEvent(SystemEventType.AutoChooseCardWithNewScopeFinished, gameId, userId.ToString());
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
      throw;
    }
  }

}