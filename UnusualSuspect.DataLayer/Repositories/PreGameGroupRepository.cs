using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories
{
	public class PreGameGroupRepository : EfRepository<PreGameGroup>, IPreGameGroupRepository
	{
		public PreGameGroupRepository(IUnitOfWork uow, ILogger<EfRepository<PreGameGroup>> logger) : base(uow, logger)
		{
		}
	}
}
