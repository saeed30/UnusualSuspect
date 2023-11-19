using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Repositories;

public class SmsLogRepository
  (IUnitOfWork uow, ILogger<EfRepository<SmsLog>> logger) : EfRepository<SmsLog>(uow, logger), ISmsLogRepository
{

}