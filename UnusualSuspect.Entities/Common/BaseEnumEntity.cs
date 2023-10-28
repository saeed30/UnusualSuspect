using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.Common;

[NotMapped]
public class BaseEnumEntity
{
	public short Id { get; set; }
	public string Name { get; set; }
	public string Title { get; set; }
}