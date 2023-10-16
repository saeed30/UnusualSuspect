using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Admin.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;
    public ConfirmEmailModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [BindProperty]
    public InputModel Input { get; set; }
    public string ReturnUrl { get; set; }
    public string UserIdTemp { get; set; }
    public class InputModel
    {
        [Required]
        public string UserId { get; set; }
        [Display(Name = "کد تائیدیه")]
        public string Code { set; get; }

    }

    public async Task<IActionResult> OnGetAsync(string userId, string returnUrl = null)
    {
        ReturnUrl = returnUrl;
        UserIdTemp = userId;
        if (userId == null)
        {
            return RedirectToPage("/Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"کاربری با این مشخصات در سایت وجود ندارد '{userId}'.");
        }
        else
        {
            if (user.SendCodeDate == null || DateTime.Now.AddMinutes(-2) > user.SendCodeDate)
            {
                user.PhoneNumberValidationCode = new Random().Next(1000, 10000).ToString();
                user.SendCodeDate = DateTime.Now;
                await _userManager.UpdateAsync(user);
                //SMSHelper.Send(user.PhoneNumber, user.PhoneNumberValidationCode);
            }
        }
        return Page();
    }


    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(Input.UserId);
            if (user != null && user.PhoneNumberValidationCode == Input.Code)
            {
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return new JsonResult(new ResultAction { Success = true, Url = returnUrl });
            }
            else
                return new JsonResult(new ResultAction { Success = false, MessageList = "کد وارد شده نادرست است. لطفا کد معتبری وارد نمائید" });

        }
        return new JsonResult(new ResultAction { Success = false, MessageList = string.Join(" - ", ModelState.Values.SelectMany(x => x.Errors)) });

    }
}
