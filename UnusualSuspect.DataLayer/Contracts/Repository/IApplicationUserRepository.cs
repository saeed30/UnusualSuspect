using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IApplicationUserRepository : IAsyncRepository<ApplicationUser>
{
  Task RecalculateRaking(CancellationToken cancellationToken = default);
  Task RecalculateMonthRaking(CancellationToken cancellationToken = default);
  Task RecalculateWeekRaking(CancellationToken cancellationToken = default);
  Task RecalculateDailyRaking(CancellationToken cancellationToken = default);
}