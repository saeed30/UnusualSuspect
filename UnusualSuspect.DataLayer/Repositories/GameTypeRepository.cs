using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories
{
	public class GameTypeRepository : EfRepository<GameType, short>, IGameTypeRepository
	{
		private readonly IUnitOfWork _uow;
		private readonly DbSet<GameType> gameType;

		public GameTypeRepository(IUnitOfWork uow, ILogger<EfRepository<GameType, short>> logger) : base(uow, logger)
		{
			_uow = uow;
			gameType = uow.Set<GameType>();
		}

		public async Task<List<GameType>> GetActiveGameTypesAsync(CancellationToken cancellationToken = default)
		{
			return await gameType.Where(x => x.IsActive).ToListAsync(cancellationToken);
		}
	}
}
