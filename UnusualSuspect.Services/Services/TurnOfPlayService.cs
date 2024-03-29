using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.ApiViewModels.SignalCommandsData;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Services;

public sealed class TurnOfPlayService(IMemoryCacheService memoryCacheService,
  IGameRepository gameRepository,
  INotificationService notificationService,
  IGameCandidateRepository gameCandidateRepository,
  ILogger<TurnOfPlayService> logger,
  IOptionsSnapshot<ProjectSetting> setting
  ) : ITurnOfPlayService
{
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
      BackgroundJob.Schedule<TurnOfPlayService>(x => x.UserTurnFinishedAsync(next.UserId, gameId, cancellationToken),
        new DateTimeOffset(model.CurrentUserTurnStartedTime, TimeSpan.FromSeconds(setting.Value.GameSetting.TimeToTalkInSeconds)));
    return new UnusualSuspectServiceResult<bool>(true);
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