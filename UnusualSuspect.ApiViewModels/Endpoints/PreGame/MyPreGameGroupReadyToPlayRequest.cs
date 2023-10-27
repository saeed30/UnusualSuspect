using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels.Endpoints.PreGame
{
	[Serializable]
	public sealed class MyPreGameGroupReadyToPlayRequest
	{
		[SerializeField]
		private int preGameGroupId;

		public int PreGameGroupId
		{
			get => preGameGroupId;
			set => preGameGroupId = value;
		}
	}
}
