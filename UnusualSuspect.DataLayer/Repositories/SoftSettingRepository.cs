using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class SoftSettingRepository(IUnitOfWork uow,
    ILogger<SoftSettingRepository> logger,
    IMemoryCacheService memoryCacheService)
  : EfRepository<SoftSetting>(uow, logger), ISoftSettingRepository
{
  public async Task<SoftSetting?> GetAsync(bool ignoreCache = false, CancellationToken cancellationToken = default)
  {
    SoftSetting? setting = null;
    if (!ignoreCache)
      setting = await memoryCacheService.GetSoftSettingAsync(cancellationToken);
    return setting ?? await BaseEntity.FirstOrDefaultAsync(cancellationToken);
  }

  public SoftSetting? Get(bool ignoreCache = false)
  {
    SoftSetting? setting = null;
    if (!ignoreCache)
      setting = memoryCacheService.GetSoftSetting();
    return setting ?? BaseEntity.FirstOrDefault();
  }

  public async Task<int> ExecuteUpdateCafebazaarAccessToken(string cafebazaarAccessToken)
  {
    return await BaseEntity.ExecuteUpdateAsync(x => x.SetProperty(a => a.CafebazaarAccessToken, cafebazaarAccessToken));
  }
}