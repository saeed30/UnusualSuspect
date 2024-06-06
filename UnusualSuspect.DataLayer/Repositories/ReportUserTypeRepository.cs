using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class ReportUserTypeRepository(IUnitOfWork uow, ILogger<ReportUserTypeRepository> logger)
  : EfRepository<ReportUserType, short>(uow, logger), IReportUserTypeRepository
{
  public async Task<List<ReportUserType>> GetAllActiveAsync(CancellationToken cancellationToken = default)
  {
    return await BaseEntity.Where(x => x.IsActive).ToListAsync(cancellationToken);
  }
}