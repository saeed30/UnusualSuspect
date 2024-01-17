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
using System.Threading;

namespace UnusualSuspect.Services.Services;

public sealed class GameService(IUnitOfWork uow,
	IGameRepository gameRepository,
	IParticipateRepository participateRepository,
	ICharacterCardGameRepository characterCardGameRepository,
	INotificationService notificationService,
	IMemoryCacheService memoryCacheService,
	ITurnOfPlayService turnOfPlayService,
	IQuestionGameRepository questionGameRepository,
	IPreGameGroupRepository preGameGroupRepository,
	IJoinedPreGameRepository joinedPreGameRepository) : IGameService
{
	public async Task<UnusualSuspectServiceResult<GameGetResponse?>> GetCurrentGameAsync(int userId, CancellationToken cancellationToken = default)
	{
		Game? game = await gameRepository.GetUserCurrentGameWithDetailsAsync(userId, cancellationToken);
		if (game == null)
			return new UnusualSuspectServiceResult<GameGetResponse?>((GameGetResponse?)null);
		return new UnusualSuspectServiceResult<GameGetResponse?>(new GameGetResponse(
			game.ToGameBaseDto(), game.ToGameFlowDto(), await memoryCacheService.GetSignalRGroupOnlineUsers(game.Id.ToString()), game.GameStatusId));
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
			case RoleCardEnum.Detective:
				//do nothing
				break;
			case RoleCardEnum.MainDetective:
			case RoleCardEnum.Witness:
			case RoleCardEnum.Accomplice:
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
		if(finalGameStatus.HasValue)
			gameRepository.SetGameStatus(game, finalGameStatus.Value);
		game.FinishedTime = DateTime.Now;
		List<int> preGameGroupIds = await preGameGroupRepository.ResetGroupsStatusAfterFinishingTheGameAsync(gameId, cancellationToken);
		await joinedPreGameRepository.ResetJoinedPreGameAfterFinishingTheGameAsync(preGameGroupIds, cancellationToken);
		await notificationService.SendSignalToGameGroup(gameId, SignalCommands.GameFinished, gameId);
		memoryCacheService.ClearGameWithDetails(gameId);
		return new UnusualSuspectServiceResult<bool>(true);
	}

	public async Task<UnusualSuspectServiceResult<bool?>> ChooseCardAndGetWinCondition(int gameId, int characterCardId, int userId, CancellationToken cancellationToken = default)
	{
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
				await gameRepository.SetGameStatusAsync(gameId, GameStatusEnum.WaitingForWitnessToAnswer, cancellationToken);
				result = new UnusualSuspectServiceResult<bool?>((bool?)null);
			}
		}
		await notificationService.SendSignalToGameGroup(gameId, SignalCommands.NewCardWasChosen);
		memoryCacheService.ClearGameWithDetails(gameId);
		return result;
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
		var result = await turnOfPlayService.StartTurnOfPlayAsync(game.Id);
		if (!result.Success)
			return false;
		gameRepository.SetGameStatus(game, GameStatusEnum.Talking);
		game.CurrentUserTurnStartedTime = result.Result.CurrentUserTurnStartedTime;
		game.TalkingTurnStartedTime = result.Result.TalkingTurnStartedTime;
		game.OrderOfParticipationTalkBeginner = result.Result.OrderOfParticipationTalkBeginner;
		game.OrderOfParticipationTurnToTalk = result.Result.OrderOfParticipationTurnToTalk;
		return true;
	}

	public async Task<UnusualSuspectServiceResult<bool>> SetWitnessAnswer(int gameId, bool witnessAnswer, short questionId, int userId, CancellationToken cancellationToken = default)
	{
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
}