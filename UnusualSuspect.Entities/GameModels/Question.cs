
namespace UnusualSuspect.Entities.GameModels
{
	public class Question : BaseEntity<short>
	{
		public string QuestionContent { get; set; }
		public bool IsActive { get; set; }
	}
}
