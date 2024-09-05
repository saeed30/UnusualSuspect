
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;

namespace UnusualSuspect.ViewModels.Dto;
public record PaymentUserSearchFilterDto(int? UserId,
  string? ExceptPurchaseToken,
  StoreEnum? Store,
  int? MinAmount,
  NullableBoolValuesEnum IsValid = NullableBoolValuesEnum.None);
