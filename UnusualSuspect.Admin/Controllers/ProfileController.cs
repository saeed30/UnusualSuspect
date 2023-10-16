using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
