
namespace UnusualSuspect.Entities.GameModels;

public enum PreGameGroupStatusEnum
{
	NotReady = 0,
	Ready = 1,
	InGame = 2
}
public class PreGameGroupStatus : BaseEntity<short>
{
	public string Name { get; set; }
	public string Title { get; set; }
}