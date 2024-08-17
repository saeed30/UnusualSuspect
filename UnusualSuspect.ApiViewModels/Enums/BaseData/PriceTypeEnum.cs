using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum PriceTypeEnum
  {
    [Display(Name = "ریال")]
    Money = 1,
    [Display(Name = "الماس")]
    Gem = 2,
    [Display(Name = "سکه")]
    Coin = 3,
    [Display(Name = "آواتار")]
    Avatar = 4,
    [Display(Name = "استیکر")]
    Sticker = 5,
    [Display(Name = "بازی")]
    Game = 6,
    [Display(Name = "گروه قبل ازی")]
    PreGame = 7,
  }
}
