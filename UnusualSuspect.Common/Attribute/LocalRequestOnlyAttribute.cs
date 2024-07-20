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
    var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
    var localIp = context.HttpContext.Connection.LocalIpAddress;

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

    base.OnActionExecuting(context);
  }
}