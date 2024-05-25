using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class GameType : BaseEntityNotIdentity<short>
{
	public string Name { get; set; }
	public string Title { get; set; }
	public short NumberOfPlayers { get; set; }
	public bool IsActive { get; set; }
	public int ViewOrder { get; set; }
	public bool AllowUserToAddOtherUsers { get; set; }
}