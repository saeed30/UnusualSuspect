using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ISoftSettingRepository : IAsyncRepository<SoftSetting>
{
  Task<SoftSetting?> GetAsync(bool ignoreCache = false, CancellationToken cancellationToken = default);
  SoftSetting? Get(bool ignoreCache = false);
  Task<int> ExecuteUpdateCafebazaarAccessToken(string cafebazaarAccessToken);
}