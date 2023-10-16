using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "نام کاربری یا شماره همراه")]
    public string PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }
    public string PhoneNumberCode { set; get; }
    public string CodeValidatePhone1 { set; get; }
    public string CodeValidatePhone2 { set; get; }
    public string CodeValidatePhone3 { set; get; }
    public string CodeValidatePhone4 { set; get; }
    public string ReturnUrl { get; set; }
    public DateTime SendDate { set; get; }
}
