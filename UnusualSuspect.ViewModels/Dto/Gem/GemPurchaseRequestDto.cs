using UnusualSuspect.ApiViewModels.Enums.BaseData;

namespace UnusualSuspect.ViewModels.Dto.Gem
{
  public class GemPurchaseRequestDto
  {
    public StoreEnum Store { get; set; }
    public short GemPackageId { get; set; }
    public CafeBazaarRequestDto? CafeBazaarRequestDto { get; set; }
    public MyketRequestDto? MyketRequestDto { get; set; }
    public bool IsBySystem { get; set; }
  }
}
