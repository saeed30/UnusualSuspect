using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class PackageMapper
{
  public static List<PackageDto> ToPackageDto(this IEnumerable<AvatarPackage> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static List<PackageDto> ToPackageDto(this IEnumerable<StickerPackage> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static List<PackageDto> ToPackageDto(this IEnumerable<GemPackage> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static List<PackageDto> ToPackageDto(this IEnumerable<CoinPackage> model)
  {
    return model.Select(x => x.ToPackageDto()).ToList();
  }
  public static List<PackageDto> ToPackageDto(this IEnumerable<PackageEntity> model)
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
      Price = model.Price,
      PriceTypeEnum = model.PriceTypeId.HasValue ? (PriceTypeEnum)model.PriceTypeId.Value : null,
      ViewOrder = model.ViewOrder,
      Enabled = true
    };
  }
}