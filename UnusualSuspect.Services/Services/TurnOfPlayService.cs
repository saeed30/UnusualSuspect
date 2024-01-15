using Aspose.Cells;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public sealed class TurnOfPlayService(IMemoryCacheService memoryCacheService,
  IGameRepository gameRepository,
  INotificationService notificationService,
  IGameCandidateRepository gameCandidateRepository,
  ILogger<TurnOfPlayService> logger) : ITurnOfPlayService
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
    memoryCacheService.SetTurnOfPlay(game.Id, model);
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.PlayersStartToTalk, starter);
    return new UnusualSuspectServiceResult<TurnOfPlayTalkingState>(model.TurnOfPlayTalkingState!);
  }

  public async Task<UnusualSuspectServiceResult<bool>> UserTurnFinishedAsync(int gameId,
    short orderOfParticipation, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    TurnOfPlayGetResponse? model = await memoryCacheService.GetTurnOfPlay(game.Id);
    if (model == null || model.TurnOfPlayTalkingState == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GameIsNotInTalkingStatus));
    var next = GetNextUserOrderOfParticipation(game.Participates,
    model.TurnOfPlayTalkingState.OrderOfParticipationTurnToTalk);
    if (next == model.TurnOfPlayTalkingState.OrderOfParticipationTalkBeginner)
    {
      await notificationService.SendSignalToGameGroup(gameId, SignalCommands.EndOfTalking, gameId);
      await gameRepository.SetGameStatusAsync(gameId, GameStatusEnum.WaitingForMainDetectiveToChoose, cancellationToken);
      await gameRepository.SaveChangesAsync(cancellationToken);
      await gameCandidateRepository.ExecuteDeleteAllGameCandidatesAsync(gameId, cancellationToken);
      return new UnusualSuspectServiceResult<bool>(false);
    }
    model.TurnOfPlayTalkingState.OrderOfParticipationTurnToTalk = next;
    model.TurnOfPlayTalkingState.CurrentUserTurnStartedTime = DateTime.Now;
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.PlayerTurnChange,
      model.TurnOfPlayTalkingState.OrderOfParticipationTurnToTalk);
    //BackgroundJob.
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async Task<UnusualSuspectServiceResult<TurnOfPlayGetResponse>> GetTurnOfPlayGetAsync(
    int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameWithDetailsAsync(userId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<TurnOfPlayGetResponse>(
        new UnusualSuspectErrorResult(LogicErrorCode.UserIsNotInActiveGame));
    TurnOfPlayGetResponse? model = await memoryCacheService.GetTurnOfPlay(game.Id);
    if (model == null)
    {
      model = game.ToTurnOfPlayGetResponse();
      memoryCacheService.SetTurnOfPlay(game.Id, model);
    }
    return new UnusualSuspectServiceResult<TurnOfPlayGetResponse>(model);
  }

  public async Task ChangedCandidateCard(int userId, short? cardId, int gameId)
  {
    TurnOfPlayGetResponse? model = await memoryCacheService.GetTurnOfPlay(gameId);
    if (model == null || model.TurnOfPlayTalkingState == null)
      return;
    var candids = model.TurnOfPlayTalkingState.CandidateCard;
    if (candids.TryGetValue(userId, out short oldChoice))
    {
      if (cardId.HasValue)
      {
        if (cardId.Value == oldChoice)
          return;
        candids[userId] = cardId.Value;
      }
      else
      {
        candids.Remove(userId);
      }
    }
    else
    {
      if (!cardId.HasValue)
        return;
      candids.Add(userId, cardId.Value);
    }
    model.TurnOfPlayTalkingState.CandidateCard = candids;
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.CandidateCardChange);
  }

  private short GetStarterOrderOfParticipation(ICollection<Participate> gameParticipates)
  {
    int random = new Random().Next(0, gameParticipates.Count - 2);
    return gameParticipates
      .Where(x => x.RoleCardId != (short)RoleCardEnum.Witness)
      .ToList()[random]
      .OrderOfParticipation;
  }
  private short GetNextUserOrderOfParticipation(ICollection<Participate> participates,
    short currentOrderOfParticipation)
  {
    Participate? next = participates.Where(x => x.OrderOfParticipation > currentOrderOfParticipation)
      .MinBy(x => x.OrderOfParticipation);
    if (next != null)
      return next.OrderOfParticipation;
    next = participates.MinBy(x => x.OrderOfParticipation);
    if (next == null)
    {
      logger.LogCritical("No participants found!");
      throw new Exception("No participants found!");
    }
    return next.OrderOfParticipation;
  }

}