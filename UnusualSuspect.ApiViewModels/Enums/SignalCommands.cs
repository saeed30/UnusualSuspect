using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums
{
  public enum SignalCommands
  {
		[Display(Name = "بازی جدید ایجاد شده است")]
    NewGameStarted = 1,
		[Display(Name = "بازی به پایان رسیده است")]
    GameFinished = 2,
		[Display(Name = "کارت جدیدی انتخاب شده است")]
    NewCardWasChosen = 3,
		[Display(Name = "کاربر جدیدی به گروه قبل از بازی اضافه شده است")]
    NewUserAdded = 4,
		[Display(Name = "کاربر بازی را ترک کرده است")]
    UserLeftTheGame = 5,
		[Display(Name = "بازی وارد مرحله مکالمه شده است")]
    PlayersStartToTalk = 6,
		[Display(Name = "نوبت مکالمه کاربر به پایان رسیده است")]
    PlayerTurnChange = 7,
		[Display(Name = "پایان مکالمه کاربران در این دور")]
    EndOfTalking = 8,
		[Display(Name = "کاربر کارت کاندید خود را تغییر داد")]
    CandidateCardChange = 9,
		[Display(Name = "ارتباط یکی از بازیکن ها برقرار شد")]
    GameMemberConnected = 10,
		[Display(Name = "ارتباط یکی از بازیکن ها قطع شد")]
    GameMemberDisConnected = 11,
		[Display(Name = "شاهد پاسخ سوال را ثبت کرده است")]
    WitnessAnswered = 12,
  }
}
