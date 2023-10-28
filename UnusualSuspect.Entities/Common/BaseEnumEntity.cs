using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.Common;

public class BaseEnumEntity : BaseEntity<short>
{
	public string Name { get; set; }
	public string Title { get; set; }
}