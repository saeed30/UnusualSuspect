using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class JoinedPreGame : BaseEntity
{
	public DateTime JoinTime { get; set; }
	public bool IsOwnerOfPreGroup { get; set; }
	public int UserId { get; set; }
	[ForeignKey("UserId")]
	public virtual ApplicationUser User { get; set; }
	public int PreGameGroupId { get; set; }
	[ForeignKey("PreGameGroupId")]
	public virtual PreGameGroup PreGameGroup { get; set; }
	public short ReadyToGameStatusId { get; set; }
	[ForeignKey("ReadyToGameStatusId")]
	public virtual ReadyToGameStatus ReadyToGameStatus { get; set; }

}