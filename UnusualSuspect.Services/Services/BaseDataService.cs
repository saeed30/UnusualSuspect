using UnusualSuspect.ApiViewModels.Endpoints.BaseData;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services;

public class BaseDataService(ISoftSettingService softSettingService) : IBaseDataService
{
  public async Task<UnusualSuspectServiceResult<BaseDataGetResponse>> GetBaseDataGetResponseAsync(int userId, CancellationToken cancellationToken = default)
  {
    SoftSetting setting = await softSettingService.GetSoftSettingAsync(false, cancellationToken);
    return new UnusualSuspectServiceResult<BaseDataGetResponse>(new BaseDataGetResponse()
    {
      Version = setting.Version
    });
  }
}