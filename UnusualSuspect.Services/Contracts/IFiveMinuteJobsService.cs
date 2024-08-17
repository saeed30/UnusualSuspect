
namespace UnusualSuspect.Services.Contracts;
public interface IFiveMinuteJobsService
{
  Task DeleteExpiredPregameGroupsAsync(CancellationToken cancellationToken);
}
