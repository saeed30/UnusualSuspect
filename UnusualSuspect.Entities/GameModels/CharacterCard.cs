using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class CharacterCard : BaseEntity<short>
{
	public string	Title { get; set; }
	public bool IsActive { get; set; }
	public string ImageUrl { get; set; }
}