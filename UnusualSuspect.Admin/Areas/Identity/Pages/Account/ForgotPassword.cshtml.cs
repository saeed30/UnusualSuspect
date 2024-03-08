using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Admin.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    public ForgotPasswordModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name ="شماره همراه")]
        public string PhoneNumber { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(Input.PhoneNumber);
            if (user == null || user.PhoneNumberConfirmed==false)
            {
                return new JsonResult(new ResultAction { Success = false, MessageList = "کاربری با این شماره موبایل در سامانه وجود ندارد." });
            }
            else
            {
                var Code = new Random().Next(1000,10000).ToString();
                await _userManager.UpdateAsync(user);
                //if (SMSHelper.Send(user.PhoneNumber, Code))
                //{
                //    return new JsonResult(new ResultAction { Success = true, Url = Url.Action("ForgetPasswordConfrimCode", "Account", new { area = "Identity"   , user.PhoneNumber}) });
                //}
                //else
                //{
                //    return new JsonResult(new ResultAction { Success = false, MessageList = "در ارسال کد تائیدیه به شماره موبایل شما خطایی رخ داده است." });
                //}
            }
        }

        return Page();
    }
}
