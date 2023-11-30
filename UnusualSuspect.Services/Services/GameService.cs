using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;
using ElmahCore;

namespace UnusualSuspect.Services.Services
{
  public sealed class GameService(IUnitOfWork uow,
    IGameRepository gameRepository,
    IParticipateRepository participateRepository,
    ICharacterCardGameRepository characterCardGameRepository) : IGameService
  {
    public async Task<UnusualSuspectServiceResult<GameGetResponse>> GetGameAsync(int gameId, int userId, CancellationToken cancellationToken = default)
    {
      bool hasAccess = await IsGameMember(gameId, userId, cancellationToken);
      if (!hasAccess)
        return new UnusualSuspectServiceResult<GameGetResponse>(new UnusualSuspectErrorResult(LogicErrorCode.AccessIsDenied));
      Game? game = await gameRepository.GetGameWithDetailsAsync(gameId, cancellationToken);
      if (game == null)
        return new UnusualSuspectServiceResult<GameGetResponse>(
          new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
      return new UnusualSuspectServiceResult<GameGetResponse>(new GameGetResponse(game.ToGameBaseDto(), game.ToGameFlowDto()));
    }

    public async Task<UnusualSuspectServiceResult<bool>> FinishGameAsync(int gameId, int userId, CancellationToken cancellationToken = default)
    {
      bool hasAccess = await IsMainDetective(gameId, userId, cancellationToken);
      if (!hasAccess)
        return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.AccessIsDenied));
      bool done = await gameRepository.SetGameFinishTimeAsync(gameId, DateTime.Now, cancellationToken);
      if (done)
      {
        //notify members
      }
      return new UnusualSuspectServiceResult<bool>(done);
    }

    public async Task<UnusualSuspectServiceResult<bool?>> ChooseCardAndGetWinCondition(int gameId, int characterCardId, int userId, CancellationToken cancellationToken = default)
    {
      bool hasAccess = await IsMainDetective(gameId, userId, cancellationToken);
      if (!hasAccess)
        return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.AccessIsDenied));
      var cards = await characterCardGameRepository.GetAllGameCharacterCardsAsync(gameId, cancellationToken);
      if(cards.Any(x=>x.CharacterCardId == characterCardId))
        return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.CharacterCardIdNotFoundInTheGame));
      var card = cards.FirstOrDefault(x => x.CharacterCardId == characterCardId && x.IsActive);
      if(card == null)
        return new UnusualSuspectServiceResult<bool?>(new UnusualSuspectErrorResult(LogicErrorCode.CharacterCardIsNotActiveInTheGame));
      if (card.IsMurderer)
      {
        await gameRepository.SetGameWinStateAsync(gameId, false, cancellationToken);
        return new UnusualSuspectServiceResult<bool?>(false);
      }
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
        return new UnusualSuspectServiceResult<bool?>(true);
      }
      return new UnusualSuspectServiceResult<bool?>((bool?)null);
    }

    private async Task<bool> IsGameMember(int gameId, int userId, CancellationToken cancellationToken = default)
    {
      return await participateRepository.IsGameParticipant(gameId, userId, cancellationToken);
    }
    private async Task<bool> IsMainDetective(int gameId, int userId, CancellationToken cancellationToken = default)
    {
      var role = await participateRepository.GetParticipantRoleAsync(gameId, userId, cancellationToken);
      if(role == null)
        return false;
      return role.Value == RoleCardEnum.MainDetective;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      return await uow.SaveChangesAsync(cancellationToken);
    }
  }
}
