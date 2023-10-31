using System;
using System.Collections.Generic;
using System.Text;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.ApiViewModels.Game
{
	public class GameParticipantDto
	{
		public int Id { get; set; }
		public short OrderOfParticipation { get; set; }
		public GameUserDto GameUserDto { get; set; }
		public GameRole GameRole {
			get;
			set;
		}

	}
}
