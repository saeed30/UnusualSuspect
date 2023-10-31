using System;
using System.Collections.Generic;
using System.Text;

namespace UnusualSuspect.ApiViewModels.Game
{
	public class GameFlowDto
	{
		public int Id { get; set; }
		public List<int> ActiveCharacterIds { get; set; }
	}
}
