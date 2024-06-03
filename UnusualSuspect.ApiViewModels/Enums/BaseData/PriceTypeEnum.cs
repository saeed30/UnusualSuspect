using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum PriceTypeEnum
  {
    [Display(Name = "تومان")]
    Money = 1,
    [Display(Name = "الماس")]
    Gem = 2,
    [Display(Name = "سکه")]
    Coin = 3,
  }
}
