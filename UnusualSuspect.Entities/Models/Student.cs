using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Entities.Models;

/// <summary>
/// دانش آموز 
/// </summary>
public  class Student : BaseEntity
{
    public Student()
    {
        FirstName = "";
        LastName = "";
        NationalCode = "";
        MobileNumber = "";
        HomePhoneNumber = "";
        FatherPhoneNumber = "";
        MotherPhoneNumber = "";
        FatherJob = "";
        Address = "";
        FirstName = "";
    }
    [Display(Name = "نام ")]
    public string FirstName { set; get; }
    [Display(Name = "نام خانوادگی ")]
    public string LastName { set; get; }
    [Display(Name = "کد ملی ")]
    public string NationalCode { set; get; }
    [Display(Name = "تاریخ تولد ")]
    public DateTime BirthDay { set; get; }
    [Display(Name = "شماره همراه ")]
    public string MobileNumber { set; get; }
    [Display(Name = "شماره منزل ")]
    public string HomePhoneNumber { set; get; }
    [Display(Name = "شماره پدر ")]
    public string FatherPhoneNumber { set; get; }
    [Display(Name = "سطح تصحیلی پدر ")]
    public int? FatherEducationLevelId { set; get; }
    [ForeignKey("FatherEducationLevelId")]
    public virtual EducationLevel? FatherEducationLevel { set; get; }
    [Display(Name = "شماره مادر ")]
    public string MotherPhoneNumber { set; get; }
    [Display(Name = "سطح تصحیلی مادر ")]
    public int? MotherEducationLevelId { set; get; }
    [ForeignKey("MotherEducationLevelId")]
    public virtual EducationLevel? MotherEducationLevel { set; get; }
    [Display(Name = "شغل پدر ")]
    public string FatherJob { set; get; }
    [Display(Name = "آدرس ")]
    public string Address { set; get; }
    [Display(Name = "تصویر ")]
    public int? ImageDocumentId { get; set; }
    [ForeignKey("ImageDocumentId")]
    public virtual Document? ImageDocument { get; set; }
    [Display(Name = "وضعیت ازدواج پدر و مادر ")]
    public int? ParentsMarriageStatusId { get; set; }
    [ForeignKey("ParentsMarriageStatusId")]
    public virtual ParentsMarriageStatus? ParentsMarriageStatus { get; set; }
    [Display(Name = "سرپرست")]
    public int? GuardianId { get; set; }
    [ForeignKey("GuardianId")]
    public virtual Guardian? Guardian { get; set; }
    public virtual ICollection<StudentInfo> StudentInfos { get; set; }

    [NotMapped]
    public string ImageDocumentBase64
    { 
        get 
        {
            if (ImageDocument == null || ImageDocument.File == null)
                return "";
            string imageBase64Data = Convert.ToBase64String(ImageDocument.File);
            return string.Format("data:image/"+ ImageDocument.DocumentType + ";base64,{0}", imageBase64Data);
        }
    }


}
