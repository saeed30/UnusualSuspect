using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class ReportUserRepository(IUnitOfWork uow, ILogger<ReportUserRepository> logger)
  : EfRepository<ReportUser>(uow, logger), IReportUserRepository
{
  public async Task<int> ReportCountSavedByThisUserForPastDayAsync(int userId, CancellationToken cancellationToken = default)
  {
    return await BaseEntity.CountAsync(x => x.UserId == userId && x.DateTimeAdded > DateTime.Now.AddDays(-1), cancellationToken);
  }
}