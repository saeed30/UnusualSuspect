using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public sealed class UserReadyStatusPreGameGroupRequest
	{
		[SerializeField]
		private int preGameGroupId;

		[SerializeField]
		private short readyToGameStatusId;

		public int PreGameGroupId
		{
			get => preGameGroupId;
			set => preGameGroupId = value;
		}

		public short ReadyToGameStatusId
		{
			get => readyToGameStatusId;
			set => readyToGameStatusId = value;
		}
	}

}
