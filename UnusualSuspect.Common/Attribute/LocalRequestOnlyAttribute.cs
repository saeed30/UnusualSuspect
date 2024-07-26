using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Serilog;

namespace UnusualSuspect.Common.Attribute;

public class LocalRequestOnlyAttribute : ActionFilterAttribute
{
  public override void OnActionExecuting(ActionExecutingContext context)
  {
    if (context.HttpContext.User.Identity == null || !context.HttpContext.User.Identity.IsAuthenticated)
    {
      context.Result = new StatusCodeResult(StatusCodes.Status402PaymentRequired);
    }
    else
    {
      string? userName = context.HttpContext.User.Identity.Name;
      //int userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt();
      var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
      var localIp = context.HttpContext.Connection.LocalIpAddress;

      if (userName == null || userName.ToLower() != "admin")
      {
        Log.Warning("LocalRequestOnlyAttribute invalid username ({userName})", userName);
        context.Result = new StatusCodeResult(StatusCodes.Status423Locked);
      }
      if (remoteIp == null || localIp == null)
      {
        Log.Warning("LocalRequestOnlyAttribute Ip Is Null");
        context.Result = new StatusCodeResult(StatusCodes.Status418ImATeapot);
      }
      else if (!IPAddress.IsLoopback(remoteIp) && remoteIp.ToString() != localIp.ToString())
      {
        Log.Warning("LocalRequestOnlyAttribute Ip Is Not Local");
        context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
      }
    }
    base.OnActionExecuting(context);
  }
}