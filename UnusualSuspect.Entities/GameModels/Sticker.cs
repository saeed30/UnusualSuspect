using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels
{
  public class Sticker : BaseEntity<short>
  {
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool IsFree { get; set; }
  }
}
