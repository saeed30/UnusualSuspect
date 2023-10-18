
namespace UnusualSuspect.Entities.GameModels
{
	public enum GameTypeEnum
	{
	}
	public class GameType : BaseEntity
	{
		public string Name { get; set; }
		public short NumberOfPlayers { get; set; }
	}
}
