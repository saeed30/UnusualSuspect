using School.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Entities.Models;

/// <summary>
/// کاربران پنل مدیریت
/// </summary>
public class AdminPanleUser : BaseEntity
{
    [Display(Name = "کد کاربر")]
    public int UserId { set; get; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser ApplicationUser { set; get; }
}
