using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Admin.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel(UserManager<ApplicationUser> userManager) : PageModel
{
  public string PhoneNumber { get; set; }

  public bool DisplayConfirmAccountLink { get; set; }

  public string EmailConfirmationUrl { get; set; }

  public async Task<IActionResult> OnGetAsync(string? phoneNumber)
  {
    if (phoneNumber == null)
    {
      return RedirectToPage("/Index");
    }

    var user = await userManager.FindByNameAsync(phoneNumber);
    if (user == null)
    {
      return NotFound($"کاربری با شماره موبایل وارد شده وجود ندارد '{phoneNumber}'.");
    }

    this.PhoneNumber = phoneNumber;
    // Once you add a real email sender, you should remove this code that lets you confirm the account
    DisplayConfirmAccountLink = true;
    if (DisplayConfirmAccountLink)
    {
      var userId = await userManager.GetUserIdAsync(user);
      var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
      EmailConfirmationUrl = Url.Page(
          "/Account/ConfirmEmail",
          pageHandler: null,
          values: new { area = "Identity", userId = userId, code = code },
          protocol: Request.Scheme);
    }

    return Page();
  }
}
