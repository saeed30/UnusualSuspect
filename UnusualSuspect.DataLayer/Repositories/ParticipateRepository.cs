using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public class ParticipateRepository
  (IUnitOfWork uow, ILogger<ParticipateRepository> logger) : EfRepository<Participate>(uow, logger),
    IParticipateRepository
{

}