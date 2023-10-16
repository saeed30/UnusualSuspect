using System;
using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ViewModels.Api.User;

public class RegisterMobileViewModel
{

    [Range(1, int.MaxValue, ErrorMessage = "شناسه کاربری نامعتبر می باشد")]
    [Required(ErrorMessage ="شناسه کاربری الزامی می باشد")]
    public int UserId { get; set; }

    [Required(ErrorMessage ="توکن الزامی می باشد")]
    [StringLength(4, ErrorMessage = "توکن باید 4 رقم باشد", MinimumLength = 4)]
    public string Token { get; set; }
    public string AppVersion { get; set; }
}
