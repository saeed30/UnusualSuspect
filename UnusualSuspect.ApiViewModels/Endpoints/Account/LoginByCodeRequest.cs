using System;

namespace UnusualSuspect.ApiViewModels.Endpoints.Account
{
	[Serializable]
	public class LoginByCodeRequest
	{
		public string Username { get; set; }
		public string Code { get; set; }
	}
}