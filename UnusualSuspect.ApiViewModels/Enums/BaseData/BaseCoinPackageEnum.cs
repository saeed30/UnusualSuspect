using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum BaseCoinPackageEnum
  {
    [Display(Name = "پاداش ثبت نام")]
    SignUpAward = 1,
    [Display(Name = "پاداش برد یا باخت در بازی")]
    GameAward = 2,
    [Display(Name = "پاداش ورود روزانه")]
    DailyAward = 3
  }
}
