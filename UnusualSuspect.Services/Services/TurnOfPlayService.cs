using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services;

public sealed class TurnOfPlayService(IMemoryCacheService memoryCacheService, IGameRepository gameRepository,
  INotificationService notificationService, ILogger<TurnOfPlayService> logger) : ITurnOfPlayService
{
  public async Task<UnusualSuspectServiceResult<bool>> StartTurnOfPlayAsync(int gameId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
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
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async Task<UnusualSuspectServiceResult<bool>> UserTurnFinishedAsync(int gameId,
    short orderOfParticipation, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    TurnOfPlayGetResponse? model = await memoryCacheService.GetTurnOfPlay(game.Id);
    if (model == null || !model.IsTalkingTime || model.TurnOfPlayTalkingState == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GameIsNotInTalkingStatus));
    model.TurnOfPlayTalkingState.OrderOfParticipationTurnToTalk = GetNextUserOrderOfParticipation(game.Participates,
      model.TurnOfPlayTalkingState.OrderOfParticipationTurnToTalk);
    model.TurnOfPlayTalkingState.CurrentUserTurnStartedTime = DateTime.Now;
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.PlayerTurnChange,
      model.TurnOfPlayTalkingState.OrderOfParticipationTurnToTalk);
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async Task<UnusualSuspectServiceResult<TurnOfPlayGetResponse>> GetTurnOfPlayGetAsync(
    int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameAsync(userId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<TurnOfPlayGetResponse>(
        new UnusualSuspectErrorResult(LogicErrorCode.UserIsNotInActiveGame));
    TurnOfPlayGetResponse? model = await memoryCacheService.GetTurnOfPlay(game.Id);
    if (model == null)
    {
      model = new TurnOfPlayGetResponse();
      memoryCacheService.SetTurnOfPlay(game.Id, model);
    }
    return new UnusualSuspectServiceResult<TurnOfPlayGetResponse>(model);
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