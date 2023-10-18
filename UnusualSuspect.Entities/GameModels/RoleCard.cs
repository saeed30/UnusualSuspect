
namespace UnusualSuspect.Entities.GameModels
{
	public enum RoleCardEnum
	{

	}
	public class RoleCard : BaseEntity
	{
		public RoleCard()
		{
			IsActive = true;
		}
		public string	Title { get; set; }
		public bool IsActive { get; set; }
		public string ImageUrl { get; set; }
	}
}
