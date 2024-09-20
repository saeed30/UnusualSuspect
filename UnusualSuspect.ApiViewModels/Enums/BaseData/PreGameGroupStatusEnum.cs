using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum PreGameGroupStatusEnum
  {
    [Display(Name = "قبل از آمادگی جهت بازی")]
    NotReady = 1,
    [Display(Name = "آماده جهت بازی")]
    Ready = 2,
    [Display(Name = "در حال بازی")]
    InGame = 3
  }
}
