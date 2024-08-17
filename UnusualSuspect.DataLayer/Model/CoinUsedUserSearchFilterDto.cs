using UnusualSuspect.ApiViewModels.Enums.BaseData;

namespace UnusualSuspect.DataLayer.Model
{
  public record CoinUsedUserSearchFilterDto(
    int? Amount = null,
    int? UserId = null,
    PriceTypeEnum? UsedForPriceType = null,
    Guid? ReferenceGuid = null
  );
}
