using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services
{
  public sealed class FiveMinuteJobsService(IPreGameService preGameService,
    IPreGameGroupRepository preGameGroupRepository,
    ISoftSettingService softSetting,
    IUnitOfWork uow,
    INotificationService notificationService,
    ILogger<FiveMinuteJobsService> logger) : IFiveMinuteJobsService
  {
    public async Task DeleteExpiredPregameGroupsAsync(CancellationToken cancellationToken)
    {
      int expireMinutes = (await softSetting.GetSoftSettingAsync(cancellationToken)).PreGameGroupExpiresInMinutes;
      if (expireMinutes <= 0)
        return;
      PreGameGroup? group = await preGameGroupRepository.GetFirstExpiredPregameGroupWithDetailsAsync(expireMinutes, cancellationToken);
      while (group != null)
      {
        UnusualSuspectServiceResult<bool> result = await preGameService.DeleteAsync(group);
        if (result.Success && result.Result)
        {
          await uow.SaveChangesAsync(cancellationToken);
          await notificationService.SendSignalToPreGameGroup(group.Id, SignalCommands.PregameGroupWasRemoved);
          logger.LogEvent(SystemEventType.PregameGroupExpiredAndRemoved, group.Id);
        }
        group = await preGameGroupRepository.GetFirstExpiredPregameGroupWithDetailsAsync(expireMinutes, cancellationToken);
      }
    }
  }
}
