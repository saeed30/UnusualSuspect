using System.ComponentModel.DataAnnotations;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public enum PreGameGroupStatusEnum
{
	[Display(Name = "قبل از آمادگی جهت بازی")]
	NotReady = 0,
	[Display(Name = "آماده جهت بازی")]
	Ready = 1,
	[Display(Name = "در حال بازی")]
	InGame = 2
}
public class PreGameGroupStatus : BaseEnumEntity
{
}