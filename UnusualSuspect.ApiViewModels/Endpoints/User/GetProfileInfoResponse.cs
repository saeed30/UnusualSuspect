using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
	public sealed class GetProfileInfoResponse
	{
		[SerializeField]
		public string? NickName { get; set; }
		[SerializeField]
		public int? UserImageDocumentId { get; set; }
	}
}