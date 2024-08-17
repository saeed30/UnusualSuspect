using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class JoinedPreGame : GuidBaseEntity
{
	public DateTime JoinTime { get; set; }
	public bool IsOwnerOfPreGroup { get; set; }
	public int UserId { get; set; }
	public int PreGameGroupId { get; set; }
	public short ReadyToGameStatusId { get; set; }
  [DefaultValue(0)]
  public int CoinAmountUsedToEnter { get; set; }

  [ForeignKey("UserId")]
	public virtual ApplicationUser User { get; set; }
	[ForeignKey("PreGameGroupId")]
	public virtual PreGameGroup PreGameGroup { get; set; }
	[ForeignKey("ReadyToGameStatusId")]
	public virtual ReadyToGameStatus ReadyToGameStatus { get; set; }

}