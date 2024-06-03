using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.Common.Enums;

public enum SystemEventType
{
  [Display(Description = "ورود کاربر")]
  Login = 1,
  [Display(Description = "خروج کاربر")]
  Logout = 2
}