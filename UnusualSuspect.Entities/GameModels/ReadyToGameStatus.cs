using System.ComponentModel.DataAnnotations;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public enum ReadyToGameStatusEnum
{
	[Display(Name = "عدم آمادگی")]
	NotReady = 1,
	[Display(Name = "اطلاع رسانی شده جهت تایید آمادگی")]
	Notified = 2,
	[Display(Name = "آماده جهت بازی")]
	Ready = 3
}
public class ReadyToGameStatus : BaseEnumEntity
{
}