
namespace UnusualSuspect.ApiViewModels.Endpoints.Account
{
	public class LoginByCodeRequest
	{
		public string Username { get; set; }
		public string Code { get; set; }
	}
}