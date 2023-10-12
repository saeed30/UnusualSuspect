using School.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Entities.Models;

/// <summary>
/// کارکنان 
/// </summary>
public  class Staff : BaseEntity
{
    public Staff()
    {
        FirstName = "";
        LastName = "";
        NationalCode = "";
        PersonnelCode = "";
        MobileNumber = "";
        HomePhoneNumber = "";
        Address = "";
    }
    [Display(Name = "نام ")]
    public string FirstName { set; get; }
    [Display(Name = "نام خانوادگی ")]
    public string LastName { set; get; }
    [Display(Name = "کد ملی ")]
    public string NationalCode { set; get; }
    [Display(Name = "شماره پرسنلی ")]
    public string PersonnelCode { set; get; }
    [Display(Name = "تاریخ تولد ")]
    public DateTime BirthDay { set; get; }
    [Display(Name = "شماره همراه ")]
    public string MobileNumber { set; get; }
    [Display(Name = "شماره ثابت ")]
    public string HomePhoneNumber { set; get; }
    [Display(Name = "آدرس ")]
    public string Address { set; get; }
    [Display(Name = "سمت ")]
    public int PostId { get; set; }
    [Display(Name = "سمت ")]
    [ForeignKey("PostId")]
    public virtual Post Post { get; set; }
    [Display(Name = "تصویر ")]
    public int? ImageDocumentId { get; set; }
    [Display(Name = "تصویر ")]
    [ForeignKey("ImageDocumentId")]
    public Document? ImageDocument { get; set; }
    public int? UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser? ApplicationUser { get; set; }

    [NotMapped]
    public string ImageDocumentBase64
    {
        get
        {
            if (ImageDocument == null || ImageDocument.File == null)
                return "";
            string imageBase64Data = Convert.ToBase64String(ImageDocument.File);
            return string.Format("data:image/" + ImageDocument.DocumentType + ";base64,{0}", imageBase64Data);
        }
    }

}
