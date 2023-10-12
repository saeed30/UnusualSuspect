using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Entities.Models;

/// <summary>
/// اطلاعات اضافه دانش آموز
/// </summary>
public class StudentInfo : BaseEntity
{
    [Display(Name = "توضیحات ")]
    public string InfoDescription { get; set; }
    [Display(Name = "تاریخ درج ")]
    public DateTime InsertDate { get; set; }
    [Display(Name = "تاریخ آخرین ویرایش ")]
    public DateTime? LastModified { get; set; }
    [Display(Name = "نوع ")]
    public int StudentInfoTypeId { get; set; }
    [ForeignKey("StudentInfoTypeId")]
    public virtual StudentInfoType StudentInfoType { get; set; }
    public int StudentId { get; set; }
    [ForeignKey("StudentId")]
    public virtual Student Student { get; set; }

}
