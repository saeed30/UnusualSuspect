using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Admin.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResetPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;
    public ResetPasswordModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
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

        var user = await _userManager.FindByNameAsync(Input.PhoneNumber);
        if (user == null)
        {
            return new JsonResult(new ResultAction { Success = false, MessageList = "اطلاعات وارد شده نامعتبر است" });

        }

        var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
        if (result.Succeeded)
        {

            await _signInManager.SignInAsync(user, isPersistent: false);
            return new JsonResult(new ResultAction { Success = true, Url = Url.Content("~/") });
        }
        return new JsonResult(new ResultAction { Success = false, MessageList = "اطلاعات وارد شده نامعتبر است" });

    }
}
