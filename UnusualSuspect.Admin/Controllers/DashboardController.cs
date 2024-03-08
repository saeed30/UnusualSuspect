using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace UnusualSuspect.Admin.Controllers;

public class DashboardController : Controller
{
    [PersianTitle("داشبورد ")]
    [ServiceFilter(typeof(UserFilters))]
    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return View();
        }
        else
        {
            return Redirect("/Account/Login?returnUrl=" + Url.Action("Index", "Dashboard", new { area = "" }));
        }
    }
}
