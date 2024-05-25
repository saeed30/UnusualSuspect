using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels
{
  public class Avatar : BaseEntityNotIdentity<short>
  {
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool IsFree { get; set; }
    public short? AvatarPackageId { get; set; }
    [ForeignKey("AvatarPackageId")]
    public virtual AvatarPackage? AvatarPackage { get; set; }
  }
}
