using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IReportUserRepository : IAsyncRepository<ReportUser>
{
  Task<int> ReportCountSavedByThisUserForPastDayAsync(int userId, CancellationToken cancellationToken = default);
}