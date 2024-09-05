using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum StoreEnum
    {
      [Display(Name = "نامشخص")]
      Unknown = 1,
      [Display(Name = "کافه بازار")]
      Cafebazaar = 2,
      [Display(Name = "مایکت")]
      Myket = 3,
    }
}
