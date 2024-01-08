using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum GameStatusEnum
  {
    [Display(Name = "در انتظار بازیکنان جهت شروع بازی")]
    WaitingForPlayers = 1,
    [Display(Name = "صحبت های قبل از انتخاب")]
    Talking = 2,
    [Display(Name = "در انتظار کارآگاه ستاره جهت انتخاب")]
    WaitingForMainDetectiveToChoose = 3,
    [Display(Name = "پایان با پیروزی")]
    FinishedAndWonTheGame = 4,
    [Display(Name = "پایان با شکست")]
    FinishedAndLostTheGame = 5,
    [Display(Name = "در انتظار شاهد جهت پاسخ به سوال")]
    WaitingForWitnessToAnswer = 6
  }

}
