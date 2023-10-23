using Microsoft.AspNetCore.Http;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	public sealed class SaveProfileInfoRequest
	{
		public string? NickName { get; set; }
		public IFormFile UserImage { get; set; }
	}
}