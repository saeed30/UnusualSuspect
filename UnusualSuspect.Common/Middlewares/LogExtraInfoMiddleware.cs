using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace UnusualSuspect.Common.Middlewares
{
	public class LogExtraInfoMiddleware
	{
		private readonly RequestDelegate next;

		public LogExtraInfoMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public Task Invoke(HttpContext context)
		{
			LogContext.PushProperty("UserName",
				context.User.Identity == null || !context.User.Identity.IsAuthenticated ? null : context.User.Identity.Name);
			return next(context);
		}
	}
}
