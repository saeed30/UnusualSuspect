using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class PackageMapper
{
  public static List<PackageDto> ToPackageDto(this List<AvatarPackage> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static List<PackageDto> ToPackageDto(this List<StickerPackage> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static List<PackageDto> ToPackageDto(this List<GemPackage> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static List<PackageDto> ToPackageDto(this List<CoinPackage> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static List<PackageDto> ToPackageDto(this List<PackageEntity> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static PackageDto ToPackageDto(this PackageEntity model)
  {
    return new PackageDto()
    {
      Amount = model.Amount,
      Id = model.Id,
      ImageUrl = model.ImageUrl,
      Price = model.Price
    };
  }
}