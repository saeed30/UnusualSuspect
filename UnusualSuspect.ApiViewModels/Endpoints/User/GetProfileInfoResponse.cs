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
		private Guid? userImageDocumentId;
		[SerializeField]
		private int? avatarId;
    [SerializeField]
    private bool? isMale;

    public bool? IsMale
    {
      get => isMale;
      set => isMale = value;
    }
		public int? AvatarId
		{
			get => avatarId;
			set => avatarId = value;
		}

		public string? NickName { get => nickName; set => nickName = value; }
		public Guid? UserImageDocumentGuidKey { get => userImageDocumentId; set => userImageDocumentId = value; }
	}
}