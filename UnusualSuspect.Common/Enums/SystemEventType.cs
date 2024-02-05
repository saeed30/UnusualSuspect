using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Common.Enums;

public enum SystemEventType
{
  [Display(Description = "ورود کاربر")]
  Login = 1,
  [Display(Description = "خروج کاربر")]
  Logout = 2
}