using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public class CreateGameGroupResponse
	{
		[SerializeField]
		public int GameGroupId { get; set; }
	}
}