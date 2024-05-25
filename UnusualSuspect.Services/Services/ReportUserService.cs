using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services;

public sealed class ReportUserService(IReportUserRepository reportUserRepository) : IReportUserService
{
  public async Task<UnusualSuspectServiceResult<bool>> SaveReportAsync(int reporterUserId, ReportUserRequest model, CancellationToken cancellationToken = default)
  {
    if (await reportUserRepository.ReportCountSavedByThisUserForPastDayAsync(reporterUserId, cancellationToken) > 3)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserReportCountMoreThanLimit));
    reportUserRepository.Add(new ReportUser()
    {
      DateTimeAdded = DateTime.Now,
      GameId = model.GameId == -1 ? null : model.GameId,
      ReportedUserId = model.ReportedUserId,
      UserId = reporterUserId,
      UserDescription = model.UserDescription
    });
    return new UnusualSuspectServiceResult<bool>(true);
  }
}