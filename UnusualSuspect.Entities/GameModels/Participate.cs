using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels
{
	public class Participate : BaseEntity
	{
		public short OrderOfParticipation { get; set; }
		public bool IsActive { get; set; }
		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual ApplicationUser ApplicationUser { get; set; }
		public int GameId { get; set; }
		[ForeignKey("GameId")]
		public virtual Game Game { get; set; }
		public int RoleCardId { get; set; }
		[ForeignKey("RoleCardId")]
		public virtual RoleCard RoleCard { get; set; }
		public int CharacterCardId { get; set; }
		[ForeignKey("CharacterCardId")]
		public virtual CharacterCard CharacterCard { get; set; }
	}
}
