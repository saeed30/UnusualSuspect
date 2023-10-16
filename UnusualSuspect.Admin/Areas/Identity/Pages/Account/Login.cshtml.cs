using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Settings;
using UnusualSuspect.Services.JcoSecurity;
using UnusualSuspect.Services.Services;

namespace UnusualSuspect.Admin.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly IAccessManagmentService  accessManagmentService;
    private readonly ICustomeMenuService  customeMenuService;

    public LoginModel(SignInManager<ApplicationUser> signInManager,
        ILogger<LoginModel> logger,
        IAccessManagmentService _accessManagmentService,
        ICustomeMenuService _customeMenuService,
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        accessManagmentService = _accessManagmentService;
        customeMenuService = _customeMenuService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
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
    }

    public async Task<IActionResult> OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl = returnUrl ?? Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;

        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Admin") || User.IsInRole("AdminPanelUserRole"))
                return LocalRedirect(Url.Action("Index", "Dashboard", new { area = "AdminPanel" }));
            else if (User.IsInRole("CustomerRole"))
                return LocalRedirect(Url.Action("Index", "Profile", new { area = "CustomerPanel" }));
        }
        return Page();

    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        try
        {

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.PhoneNumber, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var UserOB = await _userManager.FindByNameAsync(Input.PhoneNumber);
                    if (UserOB.PhoneNumberConfirmed == false)
                    {
                        await _signInManager.SignOutAsync();
                        return new JsonResult(new ResultAction
                        {
                            Success = false,
                            Url = Url.Action("ConfirmPhoneNumber", "Account", new { area = "Identity", returnUrl, userId = UserOB.Id }),
                            MessageList = $"شماره همراه شما اعتبار سنجی نشده است . لطفا جهت اعتبارسنجی بر روی لینک اعتبار سنجی کلیک کنید",
                            Params1 = "phonenumber-validation"
                        });
                    }
                    if (await _signInManager.UserManager.IsInRoleAsync(UserOB, "Admin") )
                        return new JsonResult(new ResultAction { Success = true, Url = Url.Action("Index", "Dashboard") });
                    if (await _signInManager.UserManager.IsInRoleAsync(UserOB, "AdminPanelUserRole"))
                    {
                        var resultAction = customeMenuService.ReturnUserMenus(Input.PhoneNumber, false).Result.FirstOrDefault();// user.ActionForUsers.FirstOrDefault(x=>x.AmAction);
                        if (resultAction != null)
                        {
                            return new JsonResult(new ResultAction
                            {
                                Success = true,
                                Url = Url.Action(resultAction.AmAction.Name, resultAction.AmAction.AMController.Name, new { area = "adminpanel" })
                            });
                        }
                        else
                            return new JsonResult(new
                            {
                                Success = false,
                                Message = "دسترسی  شما به سامانه تعریف نشده است ، برای پیگیری وضعیت حساب کاربری خود با مدیریت تماس بگیرید"
                            });
                    }
                    else if (await _signInManager.UserManager.IsInRoleAsync(UserOB, "CustomerRole"))
                        return new JsonResult(new ResultAction { Success = true, Url = string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Profile", new { area = "CustomerPanel" }) : returnUrl });
                    else
                        return new JsonResult(new ResultAction { Success = true, Url = returnUrl });
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    return new JsonResult(new ResultAction { Success = false, MessageList = "رمز عبور یا نام کاربری نادرست است." });
                }
            }
        }
        catch(Exception e)
        {
            string fsd = "";
        }
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
