using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
	public sealed class SaveProfileInfoRequest<T>
	{
		[SerializeField]
		private string? nickName;
		[SerializeField]
		private T userImage;
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
		public T UserImage { get => userImage; set => userImage = value; }
	}
}