using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public sealed class ReportUserService(IReportUserRepository reportUserRepository,
  IReportUserTypeRepository reportUserTypeRepository) : IReportUserService
{
  public async Task<UnusualSuspectServiceResult<bool>> SaveReportAsync(int reporterUserId, ReportUserRequest model, CancellationToken cancellationToken = default)
  {
    if (await reportUserRepository.ReportCountSavedByThisUserForPastDayAsync(reporterUserId, cancellationToken) > 3)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserReportCountMoreThanLimit));
    short? reportUserTypeId = model.ReportUserTypeId.ToShort();
    if (!reportUserTypeId.HasValue ||
       !await reportUserTypeRepository.ExistsByIdAsync(reportUserTypeId.Value, cancellationToken))
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidReportUserTypeId));
    reportUserRepository.Add(new ReportUser()
    {
      DateTimeAdded = DateTime.Now,
      GameId = model.GameId == -1 ? null : model.GameId,
      ReportedUserId = model.ReportedUserId,
      UserId = reporterUserId,
      UserDescription = model.UserDescription,
      ReportUserTypeId = (short)model.ReportUserTypeId
    });
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async Task<UnusualSuspectServiceResult<ReportUserTypesGetResponse>> GetAllReportUserTypes(CancellationToken cancellationToken = default)
  {
    var result = await reportUserTypeRepository.GetAllActiveAsync(cancellationToken);
    return new UnusualSuspectServiceResult<ReportUserTypesGetResponse>(
      new ReportUserTypesGetResponse()
      {
        ReportUserTypeDtos = result.ToReportUserTypeDto()
      });
  }
}