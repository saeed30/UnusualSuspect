using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
    public enum ReadyToGameStatusEnum
    {
        [Display(Name = "عدم آمادگی")]
        NotReady = 1,
        [Display(Name = "اطلاع رسانی شده جهت تایید آمادگی")]
        Notified = 2,
        [Display(Name = "آماده جهت بازی")]
        Ready = 3
    }
}
