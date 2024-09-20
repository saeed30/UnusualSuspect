
namespace UnusualSuspect.ViewModels.Dto;
public record GemPackageUserSearchFilterDto(
  int? UserId,
  short? PackageId,
  DateTime? FromTime,
  Guid? Guid
);