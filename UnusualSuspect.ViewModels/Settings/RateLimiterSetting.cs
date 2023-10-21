using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Settings
{
	public class RateLimiterSetting
	{
		public int AuthenticatedUserRequestPeriodInSeconds { get; set; }
		public int AuthenticatedUserAllowedRequestCount { get; set; }
		public int AnonymousUserRequestPeriodInSeconds { get; set; }
		public int AnonymousUserAllowedRequestCount { get; set; }
	}
}
