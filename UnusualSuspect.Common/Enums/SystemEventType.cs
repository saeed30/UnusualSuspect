using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.Common.Enums;

public enum SystemEventType
{
  [Display(Name = "ورود کاربر")]
  Login = 1,
  [Display(Name = "خروج کاربر")]
  Logout = 2,
  [Display(Name = "به دلیل وضعیت آزمایشی خروج خودکار کاربر انجام نشد")]
  LeaveCurrentGameNotDoneWhenTesting = 3,
  [Display(Name = "شروع خارج نمودن کاربر از بازی")]
  LeaveCurrentGameStartedForUser = 4,
  [Display(Name = "خروج از بازی کاربر با خطا مواجه شد")]
  LeaveCurrentGameUnsuccessful = 5,
  [Display(Name = "خروج از بازی کاربر ناموفق بود")]
  LeaveCurrentGameFailed = 6,
  [Display(Name = "خروج از بازی کاربر با موفقیت انجام شد")]
  LeaveCurrentGameDone = 7,
  [Display(Name = "شروع پایان خودکار نوبت کاربر")]
  UserTurnFinishedWithNewScopeStarted = 8,
  [Display(Name = "پایان پایان خودکار نوبت کاربر")]
  UserTurnFinishedWithNewScopeFinished = 9,
  [Display(Name = "شروع پاسخ خودکار به جای شاهد")]
  AutoAnswerQuestionWithNewScopeStarted = 10,
  [Display(Name = "کد بازی نامعتبر در پاسخ خودکار به جای شاهد")]
  AutoAnswerQuestionWithNewScopeInvalidGameId = 11,
  [Display(Name = "قاتل یافت نشد در پاسخ خودکار به جای شاهد")]
  AutoAnswerQuestionWithNewScopeMurdererNotFound = 12,
  [Display(Name = "پاسخ پیش فرض برای این کاراکتر یافت نشد")]
  DefaultAnswerNotFound = 13,
  [Display(Name = "پاسخ یافت نشد در پاسخ خودکار به جای شاهد")]
  AutoAnswerQuestionWithNewScopeDefaultAnswerNotFound = 14,
  [Display(Name = "پایان پاسخ خودکار به جای شاهد")]
  AutoAnswerQuestionWithNewScopeFinished = 15,
  [Display(Name = "شروع انتخاب انتخاب به جای کارآگاه")]
  AutoChooseCardWithNewScopeStarted = 16,
  [Display(Name = "کارآگاه ستاره دار یافت نشد در انتخاب به جای کارآگاه")]
  AutoChooseCardWithNewScopeMainDetectiveNotFound = 17,
  [Display(Name = "دریافت اطلاعات بازی با خطا مواجه شد در انتخاب به جای کارآگاه")]
  AutoChooseCardWithNewScopeErrorOnGettingGameInfo = 18,
  [Display(Name = "کاندیدا یافت نشد در انتخاب به جای کارآگاه")]
  AutoChooseCardWithNewScopeNoCandidate = 19,
}