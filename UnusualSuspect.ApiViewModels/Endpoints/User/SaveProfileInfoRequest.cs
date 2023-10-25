using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	[Serializable]
	public sealed class SaveProfileInfoRequest<T>
	{
		[SerializeField]
		public string? NickName { get; set; }
		[SerializeField]
		public T UserImage { get; set; }
	}
}