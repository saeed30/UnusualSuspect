using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

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