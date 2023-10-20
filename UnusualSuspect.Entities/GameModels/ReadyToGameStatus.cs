using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.GameModels
{
	public enum ReadyToGameStatusEnum
	{
		NotReady = 0,
		Notified = 1,
		Ready = 2
	}
	public class ReadyToGameStatus : BaseEntity<short>
	{
		public string Name { get; set; }
		public string Title { get; set; }
	}
}
