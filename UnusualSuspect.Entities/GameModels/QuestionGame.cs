using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.GameModels
{
	public class QuestionGame : BaseEntity
	{
		public int QuestionId { get; set; }
		public virtual Question Question { get; set; }
		public int GameId { get; set; }
		public virtual Game Game { get; set; }
		public int OrderOfUsage { get; set; }
	}
}
