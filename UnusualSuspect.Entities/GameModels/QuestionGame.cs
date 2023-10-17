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
		public virtual required Question Question { get; set; }
		public int GameId { get; set; }
		public virtual required Game Game { get; set; }
		public required int OrderOfUsage { get; set; }
	}
}
