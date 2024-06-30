using ElmahCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Core;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.DataLayer.Repositories;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Timer;

public enum GameTimerEnum
{
  AutoChooseCard,
  AutoAnswerQuestion,
  UserTurnFinished,
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
      default:
        throw new ArgumentOutOfRangeException(nameof(gameTimerEnum), gameTimerEnum, null);
    }
  }
  public void OnUserTimerStart(int userId, UserTimerEnum userTimerEnum)
  {
    try
    {
      TimerManagementService.OnUserTimerStop(userId);
      using (var scope = scopeFactory.CreateScope())
      {
        switch (userTimerEnum)
        {
          case UserTimerEnum.OutOfGameTimeout:
            if (setting.Value.GameSetting.TimeToTalkInSeconds <= 0)
              return;
            IGameService gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
            OnUserTimerStart(userId, TimeSpan.FromSeconds(setting.Value.GameSetting.TimeToTalkInSeconds * 3), LeaveCurrentGameWithNewScope);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(userTimerEnum), userTimerEnum, null);
        }
      }
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
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

  public static void OnGameTimerStop(int gameId)
  {
    if (_gameTimers.TryGetValue(gameId, out System.Timers.Timer? timer))
    {
      timer.Stop();
      timer.Dispose();
      _gameTimers.Remove(gameId);
    }
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

  public static void OnUserTimerStop(int userId)
  {
    if (_userTimers.TryGetValue(userId, out System.Timers.Timer? timer))
    {
      timer.Stop();
      timer.Dispose();
      _userTimers.Remove(userId);
    }
  }
  private async void LeaveCurrentGameWithNewScope(int userId)
  {
    using var scope = scopeFactory.CreateScope();
    IOptionsSnapshot<ProjectSetting> setting = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ProjectSetting>>();
    ILogger<TimerManagementService> loggerNew = scope.ServiceProvider.GetRequiredService<ILogger<TimerManagementService>>();
    if (setting.Value.IsTesting)
    {
      loggerNew.LogCritical("LeaveCurrentGame for user {userId} did not executed. Testing ...", userId);
      return;
    }

    IGameService gameServiceNew = scope.ServiceProvider.GetRequiredService<IGameService>();
    try
    {
      loggerNew.LogWarning("LeaveCurrentGame for user {userId}", userId);
      var result = await gameServiceNew.LeaveCurrentGameAsync(userId);
      if (!result.Success)
        loggerNew.LogWarning("LeaveCurrentGame for user {userId} unSuccess.", userId);
      else if (!result.Result)
        loggerNew.LogWarning("LeaveCurrentGame for user {userId} failed.", userId);
      else
      {
        await gameServiceNew.SaveChangesAsync();
        loggerNew.LogWarning("LeaveCurrentGame for user {userId} finished.", userId);
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
    loggerNew.LogWarning("UserTurnFinished for game {gameId} and user {userId}", gameId, userId);
    try
    {
      await gameServiceNew.UserTurnFinishedAsync(userId, gameId);
      loggerNew.LogWarning("UserTurnFinished for game {gameId} and user {userId} finished.", gameId, userId);
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

    loggerNew.LogWarning("AutoAnswerQuestion for game {gameId}", gameId);
    try
    {
      var game = (await gameServiceNew.GetDetailByIdAsync(gameId)).Result;
      if (game == null)
      {
        loggerNew.LogCritical("invalid gameId in AutoAnswerQuestion : {gameId}", gameId);
        return;
      }

      short questionId = GetCurrentQuestion(game.GameGetResponse);
      CharacterCardGame? murderer = await characterCardGameRepositoryNew.GetMurderer(gameId);
      if (murderer == null)
      {
        loggerNew.LogCritical("murderer not found in game! gameId: {gameId}", gameId);
        return;
      }
      var answer = await questionServiceNew.GetDefaultAnswer(murderer.CharacterCardId, questionId);
      if (!answer.Success)
      {
        loggerNew.LogCritical("Answer not found for question {questionId}", questionId);
        return;
      }
      await gameServiceNew.SetWitnessAnswer(gameId, answer.Result, questionId, null);
      await gameServiceNew.SaveChangesAsync();
      await notificationServiceNew.SendSignalToGameGroup(gameId, SignalCommands.WitnessAnswered, answer.Result);

      memoryCacheServiceNew.ClearGameWithDetails(gameId);
      loggerNew.LogWarning("AutoAnswerQuestion for game {gameId} finished.", gameId);
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

    loggerNew.LogWarning("AutoChooseCard for user {userId} in game {gameId}", userId, gameId);
    try
    {
      var participants = (await participateRepositoryNew.GetGameActiveParticipantsAsync(gameId, RoleCardEnum.MainDetective)).FirstOrDefault();
      if (participants == null)
      {
        loggerNew.LogError("MainDetective not found in game ({gameId})", gameId);
        return;
      }

      var game = await gameServiceNew.GetDetailByIdAsync(gameId);
      if (!game.Success)
      {
        loggerNew.LogError("Game not found with id ({gameId}). error: {error}", gameId, game.MainError.ToString());
        return;
      }

      List<short> activeCharacters = game.Result.GameGetResponse.GameFlowDto.ActiveCharacterIds;
      List<CandidateCardDto> candidate = game.Result.GameGetResponse.GameFlowDto.CandidateCard
        .Where(x => activeCharacters.Contains((short)x.CharacterCardId)).ToList();
      int characterId;
      UnusualSuspectServiceResult<bool?> chooseCardResult;
      if (candidate == null || !candidate.Any())
      {
        loggerNew.LogWarning("No candidate were found for auto choose card for game ({gameId})", gameId);
        characterId = new Random().Next(0, activeCharacters.Count - 1); ;
        chooseCardResult = await gameServiceNew.ChooseCardAndGetWinCondition(gameId, activeCharacters[characterId], userId);
      }
      else
      {
        var result = candidate.GroupBy(x => x.CharacterCardId)
          .Select(x => new { CharacterCardId = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).ToList();
        if (result.Count >= 2 && result[0].Count == result[1].Count)
        {
          loggerNew.LogWarning(
            "No candidate with most vote were found for auto choose card for game ({gameId}). number of candidates: {CandidateCount}",
            gameId, candidate.Count);
          var charWithMax = result.Where(x => x.Count == result[0].Count).ToList();
          characterId = new Random().Next(0, charWithMax.Count - 1);
          chooseCardResult = await gameServiceNew.ChooseCardAndGetWinCondition(gameId, charWithMax[characterId].CharacterCardId, userId);
        }
        else
          chooseCardResult = await gameServiceNew.ChooseCardAndGetWinCondition(gameId, result[0].CharacterCardId, userId);
      }
      await gameServiceNew.SaveChangesAsync();
      if (chooseCardResult.Result.HasValue)
        await notificationServiceNew.RemoveAllUsersFromGame(gameId);
      memoryCacheServiceNew.ClearGameWithDetails(gameId);
      loggerNew.LogWarning("AutoChooseCard for user {userId} in game {gameId} finished.", userId, gameId);
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
      throw;
    }
  }

}