using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.GameModels
{
	public class Question : BaseEntity
	{
		public required string QuestionContent { get; set; }
		public bool IsActive { get; set; }
	}
}
