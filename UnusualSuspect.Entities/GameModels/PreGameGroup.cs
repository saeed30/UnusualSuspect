using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.GameModels
{
	public class PreGameGroup : BaseEntity
	{
		public DateTime CreatedTime { get; set; }
		public DateTime? ReadyToGameTime { get; set; }
		//stores number of users in this preGame to speed up the queue calculations
		public short CalulatedJoinedUsers { get; set; }
		public short GameTypeId { get; set; }
		[ForeignKey("GameTypeId")]
		public virtual GameType GameType { get; set; }
	}
}
