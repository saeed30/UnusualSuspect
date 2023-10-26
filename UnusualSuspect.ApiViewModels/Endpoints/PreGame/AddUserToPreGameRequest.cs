using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public class AddUserToPreGameRequest
	{
		[SerializeField]
		private int preGameGroupId;
		[SerializeField]
		private string userPhoneNumber;

		public int PreGameGroupId { get => preGameGroupId; set => preGameGroupId = value; }
		public string UserPhoneNumber { get => userPhoneNumber; set => userPhoneNumber = value; }
	}
}
