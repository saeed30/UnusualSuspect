using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
	public sealed class GetProfileInfoResponse
	{
		[SerializeField]
		private string? nickName;
		[SerializeField]
		private int? userImageDocumentId;

		public string? NickName { get => nickName; set => nickName = value; }
		public int? UserImageDocumentId { get => userImageDocumentId; set => userImageDocumentId = value; }
	}
}