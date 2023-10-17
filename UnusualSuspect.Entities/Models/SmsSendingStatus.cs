using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.Models
{
	public class SmsSendingStatus : BaseEntity<short>
	{
		public string Name { get; set; }
	}
}
