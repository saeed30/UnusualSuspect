using Aspose.Cells;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services
{
	public class GameService : IGameService
	{
		private readonly IUnitOfWork _uow;
		private readonly DbSet<PreGameGroup> _PreGameGroup;
		private readonly DbSet<GameType> _GameType;
		private readonly DbSet<JoinedPreGame> _JoinedPreGame;
		private readonly DbSet<ReadyToGameStatus> _ReadyToGameStatus;
		public GameService(IUnitOfWork uow)
		{
			_uow = uow;
			_PreGameGroup = uow.Set<PreGameGroup>();
			_GameType = uow.Set<GameType>();
			_JoinedPreGame = uow.Set<JoinedPreGame>();
			_ReadyToGameStatus = uow.Set<ReadyToGameStatus>();
		}
		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await _uow.SaveChangesAsync(cancellationToken);
		}
		public async Task<PreGameGroup> StartPreGameGroup(int userId, short gameTypeId, CancellationToken cancellationToken = default)
		{
			var oldPreGames = await _JoinedPreGame.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
			if (oldPreGames.Any())
				await RemoveFromAllUserPreGames(oldPreGames, userId, cancellationToken);
			PreGameGroup group = new PreGameGroup()
			{
				CalulatedJoinedUsers = 1,
				CreatedTime = DateTime.Now,
				GameTypeId = gameTypeId,
				ReadyToGameTime = null
			};
			await _PreGameGroup.AddAsync(group, cancellationToken);
			JoinedPreGame joinedPreGame = new JoinedPreGame()
			{
				UserId = userId,
				IsOwnerOfPreGroup = true,
				JoinTime = DateTime.Now,
				PreGameGroup = group,
				ReadyToGameStatusId = (int)ReadyToGameStatusEnum.Ready
			};
			await _JoinedPreGame.AddAsync(joinedPreGame, cancellationToken);
			return group;
		}

		public async Task RemoveFromAllUserPreGames(List<JoinedPreGame> joinedPreGame, int userId, CancellationToken cancellationToken = default)
		{
			if(joinedPreGame ==  null || !joinedPreGame.Any())
				return;
			for (int i = 0; i < joinedPreGame.Count; i++)
				await RemoveUserFromPreGame(joinedPreGame[i], userId, cancellationToken);
		}

		public async Task RemoveUserFromPreGame(JoinedPreGame joinedPreGame, int userId, CancellationToken cancellationToken = default)
		{
			if(joinedPreGame.IsOwnerOfPreGroup || !await _JoinedPreGame.AnyAsync(x=>x.UserId != userId && x.PreGameGroupId == joinedPreGame.PreGameGroupId, cancellationToken))
			{
				await RemoveUserFromPreGame(joinedPreGame.PreGameGroupId);
			}
			await _JoinedPreGame.Where(x => x.UserId == userId && x.PreGameGroupId == joinedPreGame.PreGameGroupId).ExecuteDeleteAsync(cancellationToken);
			await RecalculatePreGameGroupUsers(joinedPreGame.PreGameGroupId, cancellationToken);
		}
		public async Task RecalculatePreGameGroupUsers(int preGameGroupId, CancellationToken cancellationToken = default)
		{
			var preGame = await _PreGameGroup.Where(x => x.Id == preGameGroupId).SingleAsync(cancellationToken);
			int count = await _JoinedPreGame.CountAsync(x=>x.PreGameGroupId == preGameGroupId, cancellationToken);
			if (count > 32000)
				throw new Exception("invalid user count. preGameGroupId: " + preGameGroupId);
			preGame.CalulatedJoinedUsers = (short)count;
		}

		public async Task RemoveUserFromPreGame(int preGameGroupId, CancellationToken cancellationToken = default)
		{
			await _JoinedPreGame.Where(x => x.PreGameGroupId == preGameGroupId).ExecuteDeleteAsync(cancellationToken);
			await _PreGameGroup.Where(x => x.Id == preGameGroupId).ExecuteDeleteAsync(cancellationToken);
		}

		public async Task<JoinedPreGame> AddUserToPreGameGroup(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
		{
			JoinedPreGame joinedPreGame = new JoinedPreGame()
			{
				UserId = userId,
				IsOwnerOfPreGroup = true,
				JoinTime = DateTime.Now,
				PreGameGroupId = preGameGroupId,
				ReadyToGameStatusId = (int)ReadyToGameStatusEnum.Ready
			};
			await _JoinedPreGame.AddAsync(joinedPreGame, cancellationToken);
			return joinedPreGame;
		}
	}
}
