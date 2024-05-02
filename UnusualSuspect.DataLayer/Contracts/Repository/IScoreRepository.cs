using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;
public interface IScoreRepository : IAsyncRepository<Score>
{
  Task RecalculateUserTotalScoreAsync(CancellationToken cancellationToken = default);
  Task RecalculateUserMonthScoreAsync(CancellationToken cancellationToken = default);
  Task RecalculateUserWeekScoreAsync(CancellationToken cancellationToken = default);
  Task RecalculateUserDailyScoreAsync(CancellationToken cancellationToken = default);
}
