using UnusualSuspect.Entities.JcoSecurity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Models;

public class CustomMenu:BaseEntity<int>
{

    [Display(Name = "عنوان")]
    public string Name { set; get; }
    [Display(Name = "انتخاب بخش")]
    [ForeignKey("SoftSection")]
    public int SoftSectionId { set; get; }
    public virtual SoftSection SoftSection { set; get; }

    /// <summary>
    /// اکشن اصلی
    /// </summary>       
    [Display(Name = "اکشن مرتبط با منو")]
    public int? AMActionId { set; get; }
    [ForeignKey("AMActionId")]
    public virtual AmAction? AmAction { set; get; }

    /// <summary>
    /// اکشن نوتیفیکیشن
    /// </summary>        
    [Display(Name = "اکشن نوتیفیکیشن")]
    public int? AMNotifActionId { set; get; }
    [ForeignKey("AMNotifActionId")]
    public virtual AmAction? AmNotifAction { set; get; }


    [Display(Name = "منو پدر")]
    public int? ParentId { set; get; }
    [ForeignKey("ParentId")]
    public virtual CustomMenu? ParentCustomMenu { set; get; }
    public virtual ICollection<CustomMenu> ChildeCustomeMenus { set; get; }

    [Display(Name = "آیکون")]
    public string? PatchIcon { set; get; }

    [Display(Name = "آیکون فونت آسم")]
    public string? FontAweSomeIcon { set; get; }

    [Display(Name = "پارامتر")]
    public string? Parameter { set; get; }

    public int PositionId { set; get; }
    [Display(Name = "اکشن نوتیفیکیشن")]
    /// <summary>
    /// اکشن نوتیفیکیشن
    /// </summary>        
    public int? NotifName { set; get; }
    public string? ActionAndControllerName { set; get; }
}
/// <summary>
/// نوتیفیکیشنهای موجود در پنل مدیریت
/// </summary>
public class AdminNotification : BaseEntity
{
    public string Name { set; get; }
}
