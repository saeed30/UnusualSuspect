using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IReportUserTypeRepository : IAsyncRepository<ReportUserType, short>
{
  Task<IEnumerable<ReportUserType>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}