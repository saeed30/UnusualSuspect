using System;
using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums
{
	[Serializable]
	public enum LogicErrorCode
	{
		[Display(Name = "امکان دسترسی وجود ندارد")]
		AccessIsDenied = 1,
		[Display(Name = "کاربری در گروه وجود دارد که اعلام آمادگی نکرده است")]
		ThereIsUnreadyUserInGroup = 2,
		[Display(Name = "کد نوع بازی قابل قبول نیست")]
    InvalidGameTypeId = 3,
		[Display(Name = "کاربر درخواست دهنده صاحب گروه نیست")]
    UserDoNotOwnTheGroup = 4,
		[Display(Name = "کد گروه قبل بازی غیر قابل قبول است")]
    InvalidPreGameGroupId = 5,
		[Display(Name = "نام کاربری نامعتبر است")]
		InvalidUsername = 6,
		[Display(Name = "این کاربر از پیش وارد گروه شده است")]
    UserAlreadyJoinedPreGameGroup = 7,
		[Display(Name = "کاربر مورد نظر در درون گروه یافت نشد")]
    UserJoinedPreGameGroupNotFound = 8,
		[Display(Name = "امکان اضافه نمودن صاحب گروه به گروه وجود ندارد")]
    CanNotAddOwnToPreGameGroup = 9,
		[Display(Name = "کد وضعیت آمادگی جهت بازی نا معتبر است")]
    InvalidReadyToGameStatusId = 10,
		[Display(Name = "گروه قبل بازی هیچ عضوی ندارد")]
    PreGameGroupHasNoJoinedPreGame = 11,
		[Display(Name = "بعضی از اعضای گروه در یک بازی فعال هستند")]
    CurrentGroupUsersAreInGame = 12,
		[Display(Name = "کد وضعیت گروه قبل از بازی نامعتبر است")]
    InvalidPreGameGroupStatusId = 13,
		[Display(Name = "گروه از پیش وارد بازی شده است")]
    PreGameGroupIsInGame = 14,
		[Display(Name = "فایل کاربر مورد نظر یافت نشد")]
		DocumentNotFound = 15,
		[Display(Name = "کد بازی نا معتبر است")]
		InvalidGameId = 16,
		[Display(Name = "فایل مورد نظر یافت نشد")]
		FileNotFound = 17,
		[Display(Name = "کد کاراکتر اعلامی در این بازی یافت نشد")]
    CharacterCardIdNotFoundInTheGame = 18,
		[Display(Name = "کد کاراکتر اعلامی در این بازی فعال نیست")]
    CharacterCardIsNotActiveInTheGame = 19,
		[Display(Name = "در حال حاضر هیچ کاراکتری که نقش قاتل داشته باشد در بازی فعال نیست!")]
    NoActiveMurdererFoundInGame = 20,
		[Display(Name = "کاربر عضو این گروه قبل از بازی نیست")]
    UserNotMemberOfPreGameGroup = 21,
		[Display(Name = "کاربر در حال حاضر در یک بازی فعال است ")]
    UserIsInActiveGame = 22,
		[Display(Name = "کاربر در حال حاضر در بازی فعالی نیست")]
    UserIsNotInActiveGame = 23,
		[Display(Name = "هیچ کاربری در این بازی وجود ندارد")]
    GameHasNoParticipants = 24,
		[Display(Name = "این کاربر جزو بازیکنان این بازی نیست")]
    UserDoNotParticipateInThisGame = 25,
		[Display(Name = "هیچ کارآگاهی جهت جایگزینی یافت نشد")]
    NoDetectiveInGameToReplaceUser = 26,
		[Display(Name = "بازی در وضعیت مکالمه نیست")]
    GameIsNotInTalkingStatus = 27,
		[Display(Name = "بازی در وضعیت منظر جواب شاهد نیست")]
    GameIsNotInWaitingForWitnessToAnswerStatus = 28,
		[Display(Name = "این سوال برای این بازی ثبت نشده است")]
    NoGameQuestionWithThisGameIdAndQuestionId = 29,
		[Display(Name = "تنها صاحب گروه امکان حذف اعضای دیگر را دارد")]
    OnlyGroupOwnerCanRemoveOtherUsersFromGroup = 30,
		[Display(Name = "شماره ارسال شده جهت پایان نوبت صحبت در حال صبحت نیست")]
		UserCallingFinishTalkIsNotTalking = 31,
		[Display(Name = "کد استیکر نامعتبر است")]
    InvalidStickerId = 32,
		[Display(Name = "استیکر فعال نیست")]
    StickerIsNotActive = 33,
		[Display(Name = "کد کاربری نا معتبر است")]
    InvalidUserId = 34,
    [Display(Name = "کد کاربری اعلام شده برای دوست نا معتبر است")]
    InvalidUserIdForFriend = 35,
    [Display(Name = "کاربر مورد نظر از پیش دوست کاربر بوده است")]
    UserIsAlreadyFriendWithTargetUser = 36,
    [Display(Name = "امکان اضافه کردن خود به عنوان دوست وجود ندارد")]
    CanNotAddOwnAsFriend = 37,
    [Display(Name = "کاربر انتخابی دوست کاربر جاری نیست")]
    SpecifiedUserIsNotFriend = 38,
    [Display(Name = "بازی به اتمام نرسیده است")]
    GameNotFinished = 39}
}
