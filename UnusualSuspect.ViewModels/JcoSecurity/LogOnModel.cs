using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ViewModels.JcoSecurity;

public class LogOnModel
{
    [Required(ErrorMessage = "نام کاربری ضروری می باشد")]
    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "رمز عبور ضروری می باشد")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [Display(Name = "من را به خاطر بسپار")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }

    public bool Redirect { get; set; }
}
