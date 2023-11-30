using System.ComponentModel.DataAnnotations;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public enum RoleCardEnum
{
	[Display(Name = "کارآگاه")]
	Detective = 1,
	[Display(Name = "کارآگاه ستاره دار")]
	MainDetective = 2,
	[Display(Name = "شاهد")]
	Witness = 3,
	[Display(Name = "شریک جرم")]
	Accomplice = 4
}
public class RoleCard : BaseEnumEntity, IEntity<short>
{
	public RoleCard()
	{
		IsActive = true;
		ImageUrl = "";
	}
	public bool IsActive { get; set; }
	public string ImageUrl { get; set; }
}