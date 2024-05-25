using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class CharacterCard : BaseEntityNotIdentity<short>
{
	public string	Title { get; set; }
	public bool IsActive { get; set; }
	public string ImageUrl { get; set; }
	[NotMapped]
  public short OriginalId { get; set; }
}