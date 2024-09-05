using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ISoftSettingRepository : IAsyncRepository<SoftSetting>
{
  Task<SoftSetting?> GetAsync(CancellationToken cancellationToken = default, bool ignoreCache = false);
  SoftSetting? Get(bool ignoreCache = false);
}