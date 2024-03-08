using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnusualSuspect.ViewModels.Settings;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Admin.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgetPasswordConfrimCode : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ForgetPasswordConfrimCode(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }
        [Display(Name = "کد امنیتی")]
        public string Code { get; set; }
    }

    public IActionResult OnGet(string PhoneNumber)
    {
        Input = new InputModel
        {
            PhoneNumber = PhoneNumber
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByNameAsync(Input.PhoneNumber);
        if (user != null && user.PhoneNumberValidationCode == Input.Code)
        {
            var codeTemp = await _userManager.GeneratePasswordResetTokenAsync(user);
            return new JsonResult(new ResultAction { Success = true, Url = Url.Action("ResetPassword", "Account", new { area = "Identity", code = codeTemp, user.PhoneNumber }) });
        }
        else
        {
            return new JsonResult(new ResultAction { Success = false, MessageList = "کد وارد شده ناصحیح است. لطفا با کد دیگری وارد نمائید" });
        }
    }
}
