using System;
using System.Collections.Generic;
using System.Text;

namespace UnusualSuspect.ApiViewModels.Game
{
	public class GameCharacterDto
	{
		public int Id { get; set; }
		public short CharacterId { get; set; }
		public string Title { get; set; }
		public bool IsMurderer { get; set; }
	}
}
