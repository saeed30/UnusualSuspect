using UnusualSuspect.ApiViewModels.Game;

namespace UnusualSuspect.ApiViewModels.Endpoints.Game
{
	public class GameGetResponse
	{
		public GameGetResponse(GameBaseDto gameBaseDto, GameFlowDto gameFlowDto)
		{
			GameBaseDto = gameBaseDto;
			GameFlowDto = gameFlowDto;
		}

		public GameBaseDto GameBaseDto { get; set; }
		public GameFlowDto GameFlowDto { get; set; }
	}
}
