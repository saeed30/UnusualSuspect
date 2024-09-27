using UnusualSuspect.ApiViewModels.Endpoints.Gem;
using UnusualSuspect.ViewModels.Dto.Gem;

namespace UnusualSuspect.Services.Mapping;

public static class GemMapper
{
  public static GemPurchaseRequestDto ToGemPurchaseRequestDto(this GemPurchaseRequest model)
  {
    return new GemPurchaseRequestDto()
    {
      GemPackageId = (short)model.GemPackageId,
      Store = model.Store,
      CafeBazaarRequestDto = model.CafeBazaarRequest?.ToCafeBazaarRequestDto(),
      MyketRequestDto = model.MyketRequest?.ToMyketRequestDto()
    };
  }

  public static MyketRequestDto ToMyketRequestDto(this MyketRequest model)
  {
    return new MyketRequestDto()
    {

    };
  }
  public static CafeBazaarRequestDto ToCafeBazaarRequestDto(this CafeBazaarRequest model)
  {
    return new CafeBazaarRequestDto()
    {
      ProductId = model.ProductId,
      PurchaseToken = model.PurchaseToken
    };
  }
}