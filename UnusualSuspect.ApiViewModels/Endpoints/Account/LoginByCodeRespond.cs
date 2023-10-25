using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.Account
{
	[Serializable]
	public class LoginByCodeRespond
	{
		[SerializeField]
		private int expires_in;
		[SerializeField]
		private string token_type;
		[SerializeField]
		private string refresh_token;
		[SerializeField]
		private string access_token;

		public string Access_token { get => access_token; set => access_token = value; }
		public string Refresh_token { get => refresh_token; set => refresh_token = value; }
		public string Token_type { get => token_type; set => token_type = value; }
		public int Expires_in { get => expires_in; set => expires_in = value; }

	}
}
