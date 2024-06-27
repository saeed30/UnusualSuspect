using ElmahCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Core;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
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

  public void OnUserTimerStart(int userId, UserTimerEnum userTimerEnum)
  {
    try
    {
      using (var scope = scopeFactory.CreateScope())
      {
        switch (userTimerEnum)
        {
          case UserTimerEnum.OutOfGameTimeout:
            if (setting.Value.GameSetting.TimeToTalkInSeconds <= 0)
              return;
            IGameService gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
            OnUserTimerStart(userId, TimeSpan.FromSeconds(setting.Value.GameSetting.TimeToTalkInSeconds * 3), gameService.LeaveCurrentGame);
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
  private async void UserTurnFinishedWithNewScope(int userId, int gameId)
  {
    using var scope = scopeFactory.CreateScope();

    IGameService gameServiceNew = scope.ServiceProvider.GetRequiredService<IGameService>();
    ILogger<TimerManagementService> loggerNew = scope.ServiceProvider.GetRequiredService<ILogger<TimerManagementService>>();
    loggerNew.LogWarning("UserTurnFinished for game {gameId} and user {userId}", gameId, userId);
    try
    {
      await gameServiceNew.UserTurnFinishedAsync(userId, gameId);
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
      if (candidate == null || !candidate.Any())
      {
        loggerNew.LogWarning("No candidate were found for auto choose card for game ({gameId})", gameId);
        characterId = new Random().Next(0, activeCharacters.Count - 1); ;
        await gameServiceNew.ChooseCardAndGetWinCondition(gameId, activeCharacters[characterId], userId);
        return;
      }
      var result = candidate.GroupBy(x => x.CharacterCardId)
        .Select(x => new { CharacterCardId = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).ToList();
      if (result.Count < 2 || result[0].Count == result[1].Count)
      {
        loggerNew.LogWarning(
          "No candidate with most vote were found for auto choose card for game ({gameId}). number of candidates: {CandidateCount}",
          gameId, candidate.Count);
        characterId = new Random().Next(0, activeCharacters.Count - 1);
        await gameServiceNew.ChooseCardAndGetWinCondition(gameId, activeCharacters[characterId], userId);
        return;
      }
      await gameServiceNew.ChooseCardAndGetWinCondition(gameId, result[0].CharacterCardId, userId);
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
      throw;
    }
  }

}