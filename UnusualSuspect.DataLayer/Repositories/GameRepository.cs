using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class GameRepository: EfRepository<Game>, IGameRepository
{
	public GameRepository(IUnitOfWork uow, ILogger<GameRepository> logger) : base(uow, logger)
	{
	}
}