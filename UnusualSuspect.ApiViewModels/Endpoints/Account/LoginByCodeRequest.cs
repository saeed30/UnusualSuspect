using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Account
{
	[Serializable]
	public sealed class LoginByCodeRequest
	{
		[SerializeField]
		private string code;
		[SerializeField]
		private string username;
		public string Username { get => username; set => username = value; }
		public string Code { get => code; set => code = value; }
	}
}