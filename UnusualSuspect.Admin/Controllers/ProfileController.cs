using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UnusualSuspect.Admin.Controllers;

public class ProfileController : Controller
{
    public readonly IApplicationSignInService _ApplicationSignInManager;
    public readonly IApplicationUserManager _ApplicationUserManager;

    public ProfileController(IApplicationSignInService applicationSignInManager, IApplicationUserManager applicationUserManager)
    {
        _ApplicationSignInManager = applicationSignInManager;
        _ApplicationUserManager = applicationUserManager;
    }



    public IActionResult Logout()
    {
        _ApplicationSignInManager.SignOutAsync();

        return Redirect("/");
    }



    public ActionResult EditPassword()
    {
        return View(new ApplicationUser());
    }
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<ActionResult> EditPassword(ApplicationUser user)
    {
        user.UserName = User.Identity.Name;
        return Json(await _ApplicationUserManager.EditPassword(user));
    }

}
