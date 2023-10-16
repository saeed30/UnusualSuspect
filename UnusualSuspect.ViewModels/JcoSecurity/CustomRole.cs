using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.ViewModels.JcoSecurity;

/// <summary>
/// نقشهای دستی
/// </summary>
public class CustomRole
{
    public int Id { set; get; }

    [Display(Name = "نام")]
    public string Name { set; get; }

    [Display(Name = "عنوان فارسی")]
    public string Title { set; get; }

    [ForeignKey("UserTypeRole"), Display(Name = "نوع نقش")]
    public int UserTypeRoleId { set; get; }
    //public virtual UserTypeRole UserTypeRole { set; get; }

    //public virtual ICollection<ActionForCustomRole> ActionForCustomRoles { set; get; }
    //public virtual ICollection<CustomerRoleForUser> CustomerRoleForUsers { set; get; }

    [NotMapped]
    public bool CreateOrEdit { set; get; }

    [NotMapped]
    public string ActionList { set; get; }
    [NotMapped]
    public bool Selected { get; internal set; }



}
