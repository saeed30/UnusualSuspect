using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public sealed class MyPreGameGroupChangeReadyToPlayRequest
	{
		[SerializeField]
		private int preGameGroupId;

		private bool isReady;

		public bool IsReady
		{
			get => isReady;
			set => isReady = value;
		}

		public int PreGameGroupId
		{
			get => preGameGroupId;
			set => preGameGroupId = value;
		}
	}
}
