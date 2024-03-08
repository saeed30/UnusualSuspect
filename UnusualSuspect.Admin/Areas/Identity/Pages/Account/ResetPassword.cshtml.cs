using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Admin.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResetPasswordModel(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
  : PageModel
{
  [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "شماره همراه")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "رمز عبور جدید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور جدید")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public IActionResult OnGet(string code, string PhoneNumber)
    {
        if (code == null)
        {
            return new JsonResult(new ResultAction { Success = false, MessageList = "درخواست شما نامعتبر است." });
        }
        else
        {
            Input = new InputModel
            {
                PhoneNumber = PhoneNumber,
                Code = code
            };
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await userManager.FindByNameAsync(Input.PhoneNumber);
        if (user == null)
        {
            return new JsonResult(new ResultAction { Success = false, MessageList = "اطلاعات وارد شده نامعتبر است" });

        }

        var result = await userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
        if (result.Succeeded)
        {

            await signInManager.SignInAsync(user, isPersistent: false);
            return new JsonResult(new ResultAction { Success = true, Url = Url.Content("~/") });
        }
        return new JsonResult(new ResultAction { Success = false, MessageList = "اطلاعات وارد شده نامعتبر است" });

    }
}
