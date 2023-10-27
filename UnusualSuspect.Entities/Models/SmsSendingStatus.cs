using System.ComponentModel.DataAnnotations;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Models;

public enum SmsSendingStatusEnum
{
	[Display(Name = "ارسال موفق")]
	Success = 0, 
	[Display(Name = "ارسال نا موفق")]
	Failed = 1,
}
public class SmsSendingStatus : BaseEnumEntity
{
}