using System.ComponentModel.DataAnnotations;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public enum PreGameGroupStatusEnum
{
	[Display(Name = "قبل از آمادگی جهت بازی")]
	NotReady = 1,
	[Display(Name = "آماده جهت بازی")]
	Ready = 2,
	[Display(Name = "در حال بازی")]
	InGame = 3
}
public class PreGameGroupStatus : BaseEnumEntity
{
}