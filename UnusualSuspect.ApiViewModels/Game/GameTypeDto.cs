using System;
using System.Collections.Generic;
using System.Text;

namespace UnusualSuspect.ApiViewModels.Game
{
	public class GameTypeDto
	{
		public short Id
		{
			get;
			set;
		}

		public int NumberOfPlayers { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }

	}
}
