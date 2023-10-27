using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public sealed class CreateGameGroupResponse
	{
		[SerializeField]
		private int gameGroupId;

		public int GameGroupId { get => gameGroupId; set => gameGroupId = value; }
	}
}