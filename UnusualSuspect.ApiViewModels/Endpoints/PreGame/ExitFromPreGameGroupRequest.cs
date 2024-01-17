using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public sealed class ExitFromPreGameGroupRequest
	{
		[SerializeField]
		private int preGameGroupId;

		[SerializeField]
		private int? userId;


		public int PreGameGroupId
		{
			get => preGameGroupId;
			set => preGameGroupId = value;
		}
		public int? UserId
		{
			get => userId;
			set => userId = value;
		}
	}
}
