using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.Common
{
  public class PackageEntity : BaseEnumEntity
  {
    public int Amount { get; set; }
    public int Price { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublic { get; set; }
    public string ImageUrl { get; set; }
  }
}
