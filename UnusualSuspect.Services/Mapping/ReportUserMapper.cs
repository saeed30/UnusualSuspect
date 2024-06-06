using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.Services.Mapping;

public static class ReportUserMapper
{
  public static ReportUserTypeDto ToReportUserTypeDto(this ReportUserType value)
  {
    return new ReportUserTypeDto()
    {
      Id = value.Id,
      Name = value.Name,
      Title = value.Title
    };
  }
  public static List<ReportUserTypeDto> ToReportUserTypeDto(this List<ReportUserType> value)
  {
    return value.Select(x => x.ToReportUserTypeDto()).ToList();
  }
}