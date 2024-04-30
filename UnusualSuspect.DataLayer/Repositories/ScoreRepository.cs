using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class ScoreRepository(IUnitOfWork uow, ILogger<ScoreRepository> logger) : EfRepository<Score>(uow, logger), IScoreRepository
{

}