using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace UnusualSuspect.Common.Attribute;

public class BeforActionsAttribute : ActionFilterAttribute
{
    private readonly IHttpContextAccessor _contextAccessor;
    // private readonly IActionManager actionManager  ;

    public BeforActionsAttribute(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public bool AllowUncheck { get; set; }

    public string? SecurityKey { get; set; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        //var svc = filterContext.HttpContext.RequestServices;
        //base.OnActionExecuting(filterContext);
        //if (SecurityKey != "Login" && SecurityKey != "ChangePassword" && SecurityKey != "LogOff")
        //{
        //    if (!AllowUncheck)
        //    {
        //        var hasAcces = LearningLanguage.Infrastructure.PublicModel.HassAccess(SecurityKey, _contextAccessor.HttpContext.User.Identity.Name, context);
        //        if (!hasAcces)
        //        {

        //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        //            {
        //                action = "AccessDenied",
        //                controller = "ErrorPage"
        //                ,
        //                pm = "کاربر گرامی ؛ شما مجوز دسترسی به صفحه درخواست شده را ندارید."
        //            }));
        //        }

        //    }
        //}
    }
}
