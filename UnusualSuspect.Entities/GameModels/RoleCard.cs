using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.GameModels
{
	public enum RoleCardEnum
	{

	}
	public class RoleCard
	{
		public RoleCard()
		{
			IsActive = true;
		}
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public required string	Title { get; set; }
		public bool IsActive { get; set; }
	}
}
