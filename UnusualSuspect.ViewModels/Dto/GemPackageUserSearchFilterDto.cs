
namespace UnusualSuspect.ViewModels.Dto;
public record GemPackageUserSearchFilterDto(
  int? UserId,
  short? PackageId,
  bool OnlyToday = false
);