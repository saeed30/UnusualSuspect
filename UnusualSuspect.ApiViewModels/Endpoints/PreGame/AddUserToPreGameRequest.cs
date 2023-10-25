using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public class AddUserToPreGameRequest
	{
		[SerializeField]
		public int PreGameGroupId { get; set; }
		[SerializeField]
		public string UserPhoneNumber { get; set; }
	}
}
