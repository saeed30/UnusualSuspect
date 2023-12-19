using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;
using ElmahCore;
using UnusualSuspect.ApiViewModels.Enums.BaseData;

namespace UnusualSuspect.Services.Services;

public sealed class GameService(IUnitOfWork uow,
  IGameRepository gameRepository,
  IParticipateRepository participateRepository,
  ICharacterCardGameRepository characterCardGameRepository,
  INotificationService notificationService) : IGameService
{
  public async Task<UnusualSuspectServiceResult<GameGetResponse?>> GetCurrentGameAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameWithDetailsAsync(userId, cancellationToken);
    if (game == null)
      return new UnusualSuspectServiceResult<GameGetResponse?>((GameGetResponse?)null);
    return new UnusualSuspectServiceResult<GameGetResponse?>(new GameGetResponse(game.ToGameBaseDto(), game.ToGameFlowDto()));
  }

  public async Task<UnusualSuspectServiceResult<bool>> LeaveCurrentGameAsync(int userId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetUserCurrentGameAsync(userId, cancellationToken);
    if(game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserIsNotInActiveGame));
    return await LeaveGameAsync(game.Id, userId, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<bool>> LeaveGameAsync(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    int participantCount = await participateRepository.GetParticipantCountAsync(gameId, cancellationToken);
    if(participantCount <= 0)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GameHasNoParticipants));
    if (participantCount <= 4)
      return await FinishGameAsync(gameId, cancellationToken);
    List<Participate> participates = await participateRepository.GetActiveParticipations(userId, cancellationToken);
    Participate? participate = participates.FirstOrDefault(x => x.GameId == gameId);
    if(participate == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
    participate.IsActive = false;

    //change remaining user roles

    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.UserLeftTheGame, userId);
    return new UnusualSuspectServiceResult<bool>(true);
  }

  private async Task<UnusualSuspectServiceResult<bool>> FinishGameAsync(int gameId, CancellationToken cancellationToken = default)
  {
    Game? game = await gameRepository.GetByIdAsync(gameId, cancellationToken);
    if(game == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    game.FinishedTime = DateTime.Now;
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.GameFinished, gameId);
    return new UnusualSuspectServiceResult<bool>(true);

  }

  public async Task<UnusualSuspectServiceResult<bool?>> ChooseCardAndGetWinCondition(int gameId, int characterCardId, int userId, CancellationToken cancellationToken = default)
  {
    bool hasAccess = await IsMainDetective(gameId, userId, cancellationToken);
    if (!hasAccess)
      return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.AccessIsDenied));
    var cards = await characterCardGameRepository.GetAllGameCharacterCardsAsync(gameId, cancellationToken);
    if (cards.Any(x => x.CharacterCardId == characterCardId))
      return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.CharacterCardIdNotFoundInTheGame));
    var card = cards.FirstOrDefault(x => x.CharacterCardId == characterCardId && x.IsActive);
    if (card == null)
      return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.CharacterCardIsNotActiveInTheGame));
    UnusualSuspectServiceResult<bool?> result;
    if (card.IsMurderer)
    {
      await gameRepository.SetGameWinStateAsync(gameId, false, cancellationToken);
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
        await gameRepository.SetGameWinStateAsync(gameId, true, cancellationToken);
        result = new UnusualSuspectServiceResult<bool?>(true);
      }
      else
        result = new UnusualSuspectServiceResult<bool?>((bool?)null);
    }
    Game? game = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken);
    if (game == null)
    {
      ElmahExtensions.RaiseError(new Exception("Game not available! id: " + gameId));
      return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
    }
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.NewCardWasChosen);
    return result;
  }

  public IQueryable<Game> GetAllActiveGamesWithGameType()
  {
    return gameRepository.GetAllActiveGamesWithGameType();
  }

  private async Task<bool> IsGameMember(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    return await participateRepository.IsGameParticipantAsync(gameId, userId, cancellationToken);
  }
  private async Task<bool> IsMainDetective(int gameId, int userId, CancellationToken cancellationToken = default)
  {
    var role = await participateRepository.GetParticipantRoleAsync(gameId, userId, cancellationToken);
    if (role == null)
      return false;
    return role.Value == RoleCardEnum.MainDetective;
  }

  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return await uow.SaveChangesAsync(cancellationToken);
  }
}