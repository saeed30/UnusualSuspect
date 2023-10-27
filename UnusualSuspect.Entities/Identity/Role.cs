using UnusualSuspect.Entities.JcoSecurity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Identity;

public class Role : IdentityRole<int>, IEntity
{
    public Role()
    {
    }
    public Role(string name)
     : this()
    {
        Name = name;
    }

    [StringLength(500), Display(Name = " نام ")]
    public string? Title { get; set; }
    [ForeignKey("SoftSection"), Display(Name = "انتخاب بخش")]
    public int? SoftSectionId { set; get; }
    public virtual SoftSection? SoftSection { set; get; }

    public virtual ICollection<ActionForRole> ActionForRole { set; get; }
    //public virtual ICollection<ApplicationUserRole> ApplicationUserRole { set; get; }
    [NotMapped]
    public bool CreateOrEdit { set; get; }

    [NotMapped]
    public string ActionList { set; get; }
    [NotMapped]
    public bool Selected { get; set; }
}
