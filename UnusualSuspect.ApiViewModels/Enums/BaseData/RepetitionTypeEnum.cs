using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
    public enum RepetitionTypeEnum
    {
      [Display(Name = "بدون محدودیت")]
      NoLimit = 1,
      [Display(Name = "بدون تکرار")]
      None = 2,
      [Display(Name = "روزانه")]
      Daily = 3,
      [Display(Name = "هفتگی")]
      Weekly = 4,
      [Display(Name = "ماهانه")]
      Monthly = 5,
      [Display(Name = "سالانه")]
      Yearly = 6,
    }
}
