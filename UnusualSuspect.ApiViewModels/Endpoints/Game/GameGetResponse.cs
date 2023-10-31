using System;
using UnityEngine;
using UnusualSuspect.ApiViewModels.Game;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
	[Serializable]
	public class GameGetResponse
	{
		[SerializeField]
		private GameBaseDto gameBaseDto;
		[SerializeField]
		private GameFlowDto gameFlowDto;

		public GameGetResponse(GameBaseDto gameBaseDto, GameFlowDto gameFlowDto)
		{
			GameBaseDto = gameBaseDto;
			GameFlowDto = gameFlowDto;
		}

		public GameBaseDto GameBaseDto
		{
			get => gameBaseDto;
			set => gameBaseDto = value;
		}

		public GameFlowDto GameFlowDto
		{
			get => gameFlowDto;
			set => gameFlowDto = value;
		}
	}
}
