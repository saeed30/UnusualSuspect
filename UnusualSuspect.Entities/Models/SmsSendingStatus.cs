using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.Models
{
	public class SmsSendingStatus
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public short Id { get; set; }
		public required string Name { get; set; }
	}
}
