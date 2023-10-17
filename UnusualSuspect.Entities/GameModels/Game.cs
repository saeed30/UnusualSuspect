using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.GameModels
{
	public class Game : BaseEntity
	{
		public Game()
		{
			IsFinished = false;
		}
		public bool IsFinished { get; set; }
		public DateTime CreateTime { get; set; }
		public DateTime? FinishedTime { get; set; }
	}
}
