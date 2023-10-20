
namespace UnusualSuspect.Entities.GameModels
{
	public enum GameTypeEnum
	{
	}
	public class GameType : BaseEntity<short>
	{
		public string Name { get; set; }
		public short NumberOfPlayers { get; set; }
		public bool IsActive { get; set; }
	}
}
