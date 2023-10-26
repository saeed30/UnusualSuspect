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

		public string? NickName { get => nickName; set => nickName = value; }
		public T UserImage { get => userImage; set => userImage = value; }
	}
}