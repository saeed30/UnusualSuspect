using System;
using System.Collections.Generic;
using System.Text;
using UnusualSuspect.ApiViewModels.Endpoints.Game;

namespace UnusualSuspect.ApiViewModels.Game
{
	public class GameBaseDto
	{
		public int Id { get; set; }
		public GameTypeDto GameTypeDto { get; set; }
		public IEnumerable<GameParticipantDto> GameParticipantDto { get; set; }
		public IEnumerable<GameCharacterDto> GameCharacterDtos {
			get;
			set;
		}
	}
}
