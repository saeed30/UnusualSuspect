using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.GameModels
{
	public enum GameActionEnum
	{

	}
	public class GameAction
	{
		public GameAction()
		{
			IsActive = true;
		}
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string	Title { get; set; }
		public bool IsActive { get; set; }
	}
}
