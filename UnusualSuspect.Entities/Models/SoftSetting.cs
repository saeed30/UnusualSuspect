using System.ComponentModel.DataAnnotations;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Models;

/// <summary>
/// تنظیمات نرم افزار
/// </summary>
public class SoftSetting : BaseEntity
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

  /////////////////////////////////////////////////////////////////////////////////////////////
  [Display(Name = "سکه مورد نیاز جهت پیوستن به بازی")]
  public int CoinCostToEnterPreGame { get; set; }
  [Display(Name = "انقضا گروه قبل از بازی بر اساس دقیقه")]
  public int PreGameGroupExpiresInMinutes { get; set; }
  [Display(Name = "سکه مورد نیاز جهت پیوستن به بازی برای ایجاد کننده گروه")]
  public int CoinCostToEnterPreGameForHost { get; set; }
  public int DetectiveWinScore { get; set; }
  public int WitnessWinScore { get; set; }
  public int AccompliceWinScore { get; set; }
  public int DetectiveLooseScore { get; set; }
  public int WitnessLooseScore { get; set; }
  public int AccompliceLooseScore { get; set; }
  public int DetectiveWinCoin { get; set; }
  public int WitnessWinCoin { get; set; }
  public int AccompliceWinCoin { get; set; }
  public int DetectiveLooseCoin { get; set; }
  public int WitnessLooseCoin { get; set; }
  public int AccompliceLooseCoin { get; set; }

}
