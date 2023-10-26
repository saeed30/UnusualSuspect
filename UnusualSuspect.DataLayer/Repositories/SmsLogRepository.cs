using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Repositories
{
	public class SmsLogRepository : EfRepository<SmsLog>, ISmsLogRepository
	{
		public SmsLogRepository(IUnitOfWork uow, ILogger<EfRepository<SmsLog>> logger) : base(uow, logger)
		{
		}
	}
}
