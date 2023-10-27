using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Models;

/// <summary>
/// تنظیمات نرم افزار
/// </summary>
   public class SoftSetting:BaseEntity
{
    [Display(Name = "عنوان کسب و کار")]
    public string BussinessTitle { set; get; }

    [Display(Name = "عنوان کسب و کار به صورت کوتاه")]
    public string SmallTitle { set; get; }
    [Display(Name = "شماره تماس")]
    public string ContactUsPhoneNumber { set; get; }

    [Display(Name = "شماره موبایل")]
    public string ContactUsMobileNumber { set; get; }

    [Display(Name = "شماره اس ام اس")]
    public string SMSNumber { set; get; }

    [Display(Name = "شماره فکس")]
    public string FaxNumber { set; get; }

    [Display(Name = "آدرس")]
    public string Address { set; get; }

    [Display(Name = "ایمیل")]
    public string ContactUsEmail { set; get; }

    [Display(Name = "کد پستی")]
    public string PostalCode { get; set; }

    [Display(Name = "ادرس سایت")]
    public string SiteAdress { get; set; }

    [Display(Name = "محتوای صفحه تماس با ما"), DataType(DataType.MultilineText)]
    public string ContentContactUsPage { set; get; }


}
