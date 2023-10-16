using UnusualSuspect.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.JcoSecurity;

public class ActionForRole : BaseEntity<int>
{
    public int AmActionId { get; set; }
    public int? RoleId { get; set; }

    public virtual AmAction AmAction { get; set; }
}
public class CategorySoftwareRole : BaseEntity
{
    public string Name { set; get; }

    public string AreaName { set; get; }
}
///// <summary>
///// نقشهای نرم افزاری
///// </summary>
//public class SoftwareRole : BaseEntity
//{
//    [Display(Name = "عنوان")]
//    public string Name { set; get; }

//    [ForeignKey("ApplicationRole"), Display(Name = "نقش سیستمی")]
//    public int? ApplicationRoleId { set; get; }
//    public virtual Role ApplicationRole { set; get; }


//    [ForeignKey("SoftSection"), Display(Name = "انتخاب بخش")]
//    public int SoftSectionId { set; get; }
//    public virtual SoftSection SoftSection { set; get; }

//    public virtual ICollection<ActionForSoftwareRole> ActionForSoftwareRoles { set; get; }
//    public virtual ICollection<SoftwarerRoleForUser> SoftwarerRoleForUsers { set; get; }

//    [NotMapped]
//    public bool CreateOrEdit { set; get; }

//    [NotMapped]
//    public string ActionList { set; get; }
//    [NotMapped]
//    public bool Selected { get; set; }
//}

/// <summary>
/// کنترل های هر ناحیه
/// </summary>
public class AMAreaName : BaseEntity
{
    public string Name { set; get; }

    public string? PersionName { set; get; }
    public virtual ICollection<AMController> AMControllers { set; get; }
}

/// <summary>
/// کنترل ها
/// </summary>
public class AMController : BaseEntity
{
    public string Name { set; get; }

    public string EnglishName { set; get; }

    [ForeignKey("AMAreaName")]
    public int AMAreaNameId { set; get; }
    public virtual AMAreaName AMAreaName { set; get; }
    public int? SoftSectionId { set; get; }
    [ForeignKey("SoftSectionId")]
    public virtual SoftSection? SoftSection { set; get; }

    [NotMapped]
    public bool Selected { set; get; }

    public virtual ICollection<AmAction> AmActions { set; get; }
    public string FarsiName { get; set; }
}
/// <summary>
/// اکشن های  کنترل
/// </summary>
public class AmAction : BaseEntity
{
    public string Name { set; get; }
    public string EnglishName { set; get; }
    public string ReturnTypeName { set; get; }

    [ForeignKey("AMController")]
    public int AMControllerId { set; get; }

    public virtual AMController AMController { set; get; }

    public int? SoftSectionId { set; get; }
    [ForeignKey("SoftSectionId")]
    public virtual SoftSection? SoftSection { set; get; }

    [NotMapped]
    public bool Selected { set; get; }

    public virtual ICollection<ActionForUser> ActionForUsers { set; get; }
    public virtual ICollection<ActionForSoftwareRole> ActionForSoftwareRoles { set; get; }
    public virtual ICollection<ActionForRole> ActionForRoles { set; get; }
    public string FarsiName { get; set; }
}

/// <summary>
/// دسترسی های کاربران به هر اکشن
/// </summary>
public class ActionForUser : BaseEntity
{
    [ForeignKey("AmAction")]
    public int AmActionId { set; get; }
    public virtual AmAction AmAction { set; get; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser ApplicationUser { set; get; }
}

/// <summary>
/// نقشهای نرم افزاری
/// </summary>
public class SoftwarerRoleForUser : BaseEntity
{
    [ForeignKey("SoftwareRole")]
    public int SoftwareRoleId { set; get; }
    public virtual Role? Role { set; get; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser ApplicationUser { set; get; }
}

/// <summary>
/// اکشن های هر نقش
/// </summary>
public class ActionForSoftwareRole : BaseEntity
{
    [ForeignKey("AmAction")]
    public int AmActionId { set; get; }
    public virtual AmAction AmAction { set; get; }

    public int SoftwareRoleId { get; set; }
    [ForeignKey("SoftwareRoleId")]
    public virtual Role  Role { set; get; }
}

/// <summary>
/// Software Sections
/// </summary>
public class SoftSection : BaseEntity
{
    public string TypeName { set; get; }
    public string AreaName { set; get; }
}
