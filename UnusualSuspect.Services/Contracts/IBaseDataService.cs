using UnusualSuspect.ApiViewModels.Endpoints.BaseData;

namespace UnusualSuspect.Services.Contracts;

public interface IBaseDataService
{
  Task<UnusualSuspectServiceResult<BaseDataGetResponse>> GetBaseDataGetResponseAsync(int userId,
    CancellationToken cancellationToken = default);
}