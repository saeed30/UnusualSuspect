using UnusualSuspect.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Models;

// <summary>
/// لاگ عملیات بر روی اشیاء
/// </summary>
public class LogObject : BaseEntity
{
    public string Title { set; get; }
    //[Display(Name ="کد شی")]
    //public int ObjectId { set; get; }

    [Display(Name = "مقادیر قبلی شی")]
    public string? PerValue { set; get; }
    [Display(Name = "مقادیر جدید شی")]
    public string? NextValue { set; get; }

    public string? ObjectTypeId { set; get; }
    [ForeignKey("ObjectTypeId")]
    public virtual ObjectType? ObjectType { set; get; }

    public DateTime DateCreate { set; get; }


    [NotMapped]
    public string ObjectTypeName { set; get; }
    public int UserId { set; get; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser ApplicationUser { set; get; }
}

public class ObjectType
{
    [Key, StringLength(100)]
    public string ObjectKey { set; get; }
    [Display(Name = "عنوان")]
    public string Name { set; get; }
}

