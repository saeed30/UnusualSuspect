using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Services.Services
{
	public class GameService : IGameService
	{
		private readonly IUnitOfWork uow;
		private readonly IPreGameGroupRepository preGameGroupRepository;
		private readonly IJoinedPreGameRepository joinedPreGameRepository;
		private readonly IGameTypeRepository gameTypeRepository;
		private readonly IApplicationUserManager applicationUserManager;

		public GameService(IUnitOfWork uow,
			IPreGameGroupRepository preGameGroupRepository,
			IJoinedPreGameRepository joinedPreGameRepository,
			IApplicationUserManager applicationUserManager,
			IGameTypeRepository gameTypeRepository)
		{
			this.uow = uow;
			this.preGameGroupRepository = preGameGroupRepository;
			this.joinedPreGameRepository = joinedPreGameRepository;
			this.applicationUserManager = applicationUserManager;
			this.gameTypeRepository = gameTypeRepository;
		}
		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await uow.SaveChangesAsync(cancellationToken);
		}
		public async Task<UnusualSuspectServiceResult<PreGameGroup>> CreatePreGameGroup(int userId, short gameTypeId, CancellationToken cancellationToken = default)
		{
			GameType? gameType = await gameTypeRepository.GetByIdAsync(gameTypeId, cancellationToken);
			if (gameType == null)
				return new UnusualSuspectServiceResult<PreGameGroup>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameTypeId));
			var oldPreGames = await joinedPreGameRepository.PreGameGroupOfUserAsync(userId, cancellationToken);
			if (oldPreGames.Any())
				await RemoveFromAllUserPreGames(oldPreGames.ToList(), userId, cancellationToken);
			PreGameGroup group = new PreGameGroup()
			{
				CalculatedJoinedUsers = 1,
				CreatedTime = DateTime.Now,
				GameType = gameType,
				ReadyToGameTime = null,
				PreGameGroupStatusId = (int)PreGameGroupStatusEnum.NotReady
			};
			preGameGroupRepository.Add(group);
			JoinedPreGame joinedPreGame = new JoinedPreGame()
			{
				UserId = userId,
				IsOwnerOfPreGroup = true,
				JoinTime = DateTime.Now,
				PreGameGroup = group,
				ReadyToGameStatusId = (int)ReadyToGameStatusEnum.Ready
			};
			joinedPreGameRepository.Add(joinedPreGame);
			return new UnusualSuspectServiceResult<PreGameGroup>(group);
		}

		public async Task RemoveFromAllUserPreGames(List<JoinedPreGame> joinedPreGame, int userId, CancellationToken cancellationToken = default)
		{
			if (!joinedPreGame.Any())
				return;
			for (int i = 0; i < joinedPreGame.Count; i++)
				await RemoveUserFromPreGameGroup(joinedPreGame[i], userId, cancellationToken);
		}

		public async Task RemoveUserFromPreGameGroup(JoinedPreGame joinedPreGame, int userId, CancellationToken cancellationToken = default)
		{
			await joinedPreGameRepository.ExecuteDeleteUserJoinedPreGameGroupAsync(userId,joinedPreGame.PreGameGroupId, cancellationToken);
			if (joinedPreGame.IsOwnerOfPreGroup || !await joinedPreGameRepository.ExistsInPreGameGroupExceptUserAsync(userId, joinedPreGame.PreGameGroupId, cancellationToken))
				await RemovePreGameGroup(joinedPreGame.PreGameGroupId, cancellationToken);
			else
				await RecalculatePreGameGroupUsers(joinedPreGame.PreGameGroupId, cancellationToken);
		}
		public async Task RecalculatePreGameGroupUsers(int preGameGroupId, CancellationToken cancellationToken = default)
		{
			var preGame = await preGameGroupRepository.GetByIdAsync(preGameGroupId, cancellationToken);
			if(preGame == null)
				throw new Exception("preGameGroup not found. preGameGroupId: " + preGameGroupId);
			int count = await joinedPreGameRepository.UserCountJoinedPreGameGroupAsync(preGameGroupId, cancellationToken);
			if (count > 32000)
				throw new Exception("invalid user count. preGameGroupId: " + preGameGroupId);
			preGame.CalculatedJoinedUsers = (short)count;
		}

		public async Task RemovePreGameGroup(int preGameGroupId, CancellationToken cancellationToken = default)
		{
			await joinedPreGameRepository.ExecuteDeleteAllJoinedPreGameGroupAsync(preGameGroupId, cancellationToken);
			await preGameGroupRepository.ExecuteDeleteByIdAsync(preGameGroupId, cancellationToken);
		}
		public async Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatus(int userId, int preGameGroupId, ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default)
		{
			var preGameGroup = await preGameGroupRepository.GetByIdAsync(preGameGroupId, cancellationToken);
			if(preGameGroup == null)
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupId));
			if(preGameGroup.PreGameGroupStatusId != (int)PreGameGroupStatusEnum.NotReady)
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupStatusId));
			var joinedPreGame = await joinedPreGameRepository.GetByUserIdPreGameGroupIdAsync(userId, preGameGroupId, cancellationToken);
			if(joinedPreGame == null)
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserJoinedPreGameGroupNotFound));
			joinedPreGame.ReadyToGameStatusId = (short)readyToGameStatusEnum;
			return new UnusualSuspectServiceResult<bool>(true);
		}
		public async Task<UnusualSuspectServiceResult<bool>> StartPreGameGroup(int preGameGroupId)
		{
			if (!await joinedPreGameRepository.AllJoinedPreGameGroupUsersAreReadyAsync(preGameGroupId))
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.ThereIsUnreadyUserInGroup));
			return new UnusualSuspectServiceResult<bool>(true);
		}
		public async Task<UnusualSuspectServiceResult<JoinedPreGame>> AddUserToPreGameGroup(int addingUserId, string username, int preGameGroupId, CancellationToken cancellationToken = default)
		{
			var user = await applicationUserManager.FindByNameAsync(username);
			if(user == null)
				return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidUsername));
			if(user.Id == addingUserId)
				return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.CanNotAddOwnToPreGameGroup));
			return await AddUserToPreGameGroup(addingUserId, user.Id, preGameGroupId, cancellationToken);
		}
		public async Task<UnusualSuspectServiceResult<JoinedPreGame>> AddUserToPreGameGroup(int addingUserId, int userId, int preGameGroupId, CancellationToken cancellationToken = default)
		{
			PreGameGroup? preGameGroup = await preGameGroupRepository.GetByIdAsync(preGameGroupId, cancellationToken);
			if (preGameGroup == null)
				return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupId));
			bool ownsTheGroup = await CheckUserOwnsThePreGameGroup(addingUserId, preGameGroupId, cancellationToken);
			if(!ownsTheGroup)
				return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotOwnTheGroup));
			bool alreadyJoinedPreGameGroup = await CheckUserJoinedPreGameGroup(userId, preGameGroupId, cancellationToken);
			if(alreadyJoinedPreGameGroup)
				return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.UserAlreadyJoinedPreGameGroup));
			JoinedPreGame joinedPreGame = new JoinedPreGame()
			{
				UserId = userId,
				IsOwnerOfPreGroup = true,
				JoinTime = DateTime.Now,
				PreGameGroup = preGameGroup,
				ReadyToGameStatusId = (int)ReadyToGameStatusEnum.Notified
			};
			joinedPreGameRepository.Add(joinedPreGame);
			return new UnusualSuspectServiceResult<JoinedPreGame>(joinedPreGame);
		}


		public async Task CombineGroupsToStartGames(CancellationToken cancellationToken = default)
		{
			var gameTypes = await gameTypeRepository.GetActiveGameTypesAsync(cancellationToken);
			foreach (var gameType in gameTypes)
			{
				CombineGroupsToStartGamesByGameType(gameType, cancellationToken);
			}
		}

		public async Task<UnusualSuspectServiceResult<bool>> PreGameReadyToPlayAsync(int preGameGroupId, CancellationToken cancellationToken = default)
		{
			PreGameGroup? preGameGroup =
				await preGameGroupRepository.GetByIdWithJoinedPreGameAsync(preGameGroupId, cancellationToken);
			if(preGameGroup == null)
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupId));
			if(preGameGroup.PreGameGroupStatusId != (int)PreGameGroupStatusEnum.NotReady)
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.PreGameGroupHasNoJoinedPreGame));
			if (!preGameGroup.JoinedPreGames.Any())
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.PreGameGroupHasNoJoinedPreGame));
			if(preGameGroup.JoinedPreGames.Any(x=>x.ReadyToGameStatusId != (int)ReadyToGameStatusEnum.Ready))
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.ThereIsUnreadyUserInGroup));
			List<int> inGameUserIds = CheckNoJoinedUsersAreInGameAndDeleteInactiveJoinedPreGames(preGameGroupId);
			if(inGameUserIds.Any())
				return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.CurrentGroupUsersAreInGame));
			preGameGroup.PreGameGroupStatusId = (int)PreGameGroupStatusEnum.Ready;
			return new UnusualSuspectServiceResult<bool>(true);
		}

		private List<int> CheckNoJoinedUsersAreInGameAndDeleteInactiveJoinedPreGames(int preGameGroupId)
		{
			//throw new NotImplementedException();
			return new List<int>();
		}

		#region PrivateMethods
		private void CombineGroupsToStartGamesByGameType(GameType gameType, CancellationToken cancellationToken)
		{

		}
		private async Task<bool> CheckUserOwnsThePreGameGroup(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
		{
			JoinedPreGame? joinedPreGame = await joinedPreGameRepository.GetByUserIdPreGameGroupIdAsync(userId, preGameGroupId, cancellationToken);
			if(joinedPreGame == null)
				return false;
			return joinedPreGame.IsOwnerOfPreGroup;
		}
		private async Task<bool> CheckUserJoinedPreGameGroup(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
		{
			return await joinedPreGameRepository.UserExistsInPreGameGroupAsync(userId, preGameGroupId, cancellationToken);
		}

		#endregion PrivateMethods

	}
}
