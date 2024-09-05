
namespace UnusualSuspect.ViewModels.Dto;
public record CoinPackageUserSearchFilterDto(
  int? UserId,
  short? PackageId,
  bool OnlyToday = false
);
