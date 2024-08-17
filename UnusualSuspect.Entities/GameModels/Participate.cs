using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class Participate : GuidBaseEntity
{
	public short OrderOfParticipation { get; set; }
	public bool IsActive { get; set; }
	public int UserId { get; set; }
	public int GameId { get; set; }
	public short RoleCardId { get; set; }
  [DefaultValue(0)]
  public int CoinAmountUsedToEnter { get; set; }

  [ForeignKey("UserId")]
	public virtual ApplicationUser ApplicationUser { get; set; }
	[ForeignKey("GameId")]
	public virtual Game Game { get; set; }
	[ForeignKey("RoleCardId")]
	public virtual RoleCard RoleCard { get; set; }
}