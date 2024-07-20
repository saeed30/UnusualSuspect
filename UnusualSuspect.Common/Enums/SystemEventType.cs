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

  [Display(Name = "هیچ کاندیدایی دارای رای اکثریت نبود")]
  AutoChooseCardWithNewScopeNoCandidateWithTopVote = 20,

  [Display(Name = "خطا در زمان ثبت انتخاب خودکار. خطا:{MainError}")]
  AutoChooseCardWithNewScopeErrorOnChoose = 21,

  [Display(Name = "پایان انتخاب انتخاب به جای کارآگاه")]
  AutoChooseCardWithNewScopeFinished = 22,

  [Display(Name = "خطا در زمان ثبت نام با شماره همراه رخ داده است")]
  ErrorOnRegisterByPhone = 23,

  [Display(Name = "ثبت الماس در زمان ثبت نام با خطا مواجه شد")]
  BaseGemPackageEnumOnRegisterFailed = 24,

  [Display(Name = "ثبت سکه در زمان ثبت نام با خطا مواجه شد")]
  BaseCoinPackageEnumOnRegisterFailed = 25,

  [Display(Name = "خطای دریافتی از نرم افزار")]
  MobileAppError = 26,

  [Display(Name = "کاربر دارای بیش از یک کاندید است")]
  UserWithMultipleCandidates = 27,

  [Display(Name = "اطلاعات جلسه مکالمه درست نیست")]
  InvalidStatusDataOnTalking = 28,

  [Display(Name = "در بازی کاربر دارای بیش از یک کاندید است")]
  GameWithUserWithMultipleCandidates = 29,

  [Display(Name = "کد بازی نامعتبر است")]
  InvalidGameIdInSetNewTurnToTalk = 30,

  [Display(Name = "کاربر بیش از یک بازی فعال دارد")]
  UserHasMoreThanOneActiveGame = 31,

  [Display(Name = "پاسخ پیشفرض برای سوال و کارت مورد نظر یافت نشد")]
  NoDefaultAnswerAvailable = 32,

  [Display(Name = "مقدار الماس محاسبه شده کاربر زیر صفر است")]
  UserCalculatedGemsBelowZero = 33,

  [Display(Name = "مقدار الماس محاسبه شده با مقدار موجود برابر نیست")]
  UserCalculatedGemsNotEqualToCurrentValue = 34,

  [Display(Name = "مقدار سکه محاسبه شده کاربر زیر صفر است")]
  UserCalculatedCoinsBelowZero = 35,

  [Display(Name = "مقدار سکه محاسبه شده با مقدار موجود برابر نیست")]
  UserCalculatedCoinsNotEqualToCurrentValue = 36,

  [Display(Name = "ارسال سیگنال به گروه قبل بازی")]
  SendSignalToPreGameGroup = 37,

  [Display(Name = "ارسال سیگنال به گروه بازی")]
  SendSignalToGameGroup = 38,

  [Display(Name = "خرید بسته با پول نباید به این شکل بررسی شود")]
  PayPackageWithMoneyShouldNotBeChecked = 39,

  [Display(Name = "خرید بسته با آواتار نباید به این شکل بررسی شود")]
  PayPackageWithAvatarShouldNotBeChecked = 40,

  [Display(Name = "خرید بسته با استیکر نباید به این شکل بررسی شود")]
  PayPackageWithStickerShouldNotBeChecked = 41,

  [Display(Name = "خرید بسته با آواتار معتبر نیست")]
  PayPackageWithAvatarIsNotValid = 42,

  [Display(Name = "خرید بسته با استیکر معتبر نیست")]
  PayPackageWithStickerIsNotValid = 43,

  [Display(Name = "تعداد افراد اضافه شده به گروه قبل از بازی بیش از مقدار قابل قبول نوع بازی")]
  CalculatedJoinedUsersMoreThanTypeNumberOfPlayers = 44,

  [Display(Name = "تعداد افراد اضافه شده به بازی بیش از مقدار قابل قبول نوع بازی")]
  GameAddedParticipantsMoreThanTypeNumberOfPlayers = 45,

  [Display(Name = "شروع محاسبات رتبه بندی")]
  RecalculateAllRankingsStarted = 46,

  [Display(Name = "پایان محاسبات رتبه بندی")]
  RecalculateAllRankingsFinished = 47,

  [Display(Name = "RecalculateTotalRankingsStarted")]
  RecalculateTotalRankingsStarted = 48,

  [Display(Name = "RecalculateTotalRankingsFinished")]
  RecalculateTotalRankingsFinished = 49,

  [Display(Name = "RecalculateMonthRankingsStarted")]
  RecalculateMonthRankingsStarted = 50,

  [Display(Name = "RecalculateMonthRankingsFinished")]
  RecalculateMonthRankingsFinished = 51,

  [Display(Name = "RecalculateWeekRankingsStarted")]
  RecalculateWeekRankingsStarted = 52,

  [Display(Name = "RecalculateWeekRankingsFinished")]
  RecalculateWeekRankingsFinished = 53,

  [Display(Name = "RecalculateDailyRankingsStarted")]
  RecalculateDailyRankingsStarted = 54,

  [Display(Name = "RecalculateDailyRankingsFinished")]
  RecalculateDailyRankingsFinished = 55,

  [Display(Name = "فراخوانی سیگنال ارسال پیام")]
  GameHubSendMessage = 56,

  [Display(Name = "فراخوانی سیگنال شروع صحبت")]
  GameHubStartedToTalk = 57,

  [Display(Name = "فراخوانی سیگنال پایان صحبت")]
  GameHubFinishedTalking = 58,

  [Display(Name = "فراخوانی سیگنال استفاده از استیکر")]
  GameHubUseSticker = 59,

  [Display(Name = "فراخوانی سیگنال کاندید نمودن کارت")]
  GameHubCandidateCard = 60,

  [Display(Name = "برقراری ارتباط سیگنال")]
  GameHubOnConnectedAsync = 61,
  [Display(Name = "قطع ارتباط سیگنال")] GameHubOnDisconnectedAsync = 62,

  [Display(Name = "جدول کاربران یافت نشد")]
  SeedUserTableNotCreated = 63,

  [Display(Name = "ایجاد نقش مدیر با مشکل مواجه شد")]
  SeedAdminRoleCreateFailed = 64,

  [Display(Name = "ایجاد نقش مشتری با مشکل مواجه شد")]
  SeedCustomerRoleCreateFailed = 65,
}