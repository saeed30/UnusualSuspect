using UnusualSuspect.ApiViewModels.Endpoints.User;

namespace UnusualSuspect.Services.Contracts;

public interface IReportUserService
{
  Task<UnusualSuspectServiceResult<bool>> SaveReportAsync(int reporterUserId, ReportUserRequest model, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<ReportUserTypesGetResponse>> GetAllReportUserTypes(CancellationToken cancellationToken = default);
}