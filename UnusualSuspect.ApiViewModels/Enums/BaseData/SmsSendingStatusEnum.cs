using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum SmsSendingStatusEnum
  {
    [Display(Name = "ارسال موفق")]
    Success = 1,
    [Display(Name = "ارسال نا موفق")]
    Failed = 2,
  }
}
