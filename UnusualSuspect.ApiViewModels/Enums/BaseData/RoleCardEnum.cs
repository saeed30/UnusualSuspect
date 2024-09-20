using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum RoleCardEnum
  {
    [Display(Name = "کارآگاه")]
    Detective = 1,
    [Display(Name = "کارآگاه ستاره دار")]
    MainDetective = 2,
    [Display(Name = "شاهد")]
    Witness = 3,
    [Display(Name = "شریک جرم")]
    Accomplice = 4
  }
}
