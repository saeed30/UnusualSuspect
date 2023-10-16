
using UnusualSuspect.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;

namespace UnusualSuspect.Admin.Models;

public class UserFilters : ActionFilterAttribute
{
    private readonly IApplicationUserService _IApplicationUserService;

    public UserFilters(IApplicationUserService iApplicationUserService)
    {
        _IApplicationUserService = iApplicationUserService;
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if(filterContext.HttpContext == null || filterContext.HttpContext.User.Identity  == null
            || filterContext.HttpContext.User.Identity.Name == null || !filterContext.HttpContext.User.Identity.IsAuthenticated)
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account" }));
        else
        {

            //var currentUser = _IApplicationUserService.DetailsUser(filterContext.HttpContext.User.Identity.Name);
            //string ControllerName = (string)filterContext.RouteData.Values["Controller"];
            //string ActionName = (string)filterContext.RouteData.Values["action"];
            //if ((filterContext.HttpContext.User.IsInRole("AdminPanelUserRole"))/* && !currentUser.ActionForUsers.Any(x => (x.AmAction.AMController.Name == ControllerName) || (x.AmAction.Name == ActionName))*/)
            //{
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "LackOfAccess", controller = "Dashboard", area = "AdminPanel" }));
            //}
        }

        base.OnActionExecuting(filterContext);
    }
}
