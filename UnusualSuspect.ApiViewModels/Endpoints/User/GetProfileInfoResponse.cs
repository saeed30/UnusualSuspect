using System;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
	public sealed class GetProfileInfoResponse
	{
		public string? NickName { get; set; }
		public int? UserImageDocumentId { get; set; }
	}
}