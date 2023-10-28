using System.ComponentModel.DataAnnotations;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.Models;

public enum SmsSendingStatusEnum
{
	[Display(Name = "ارسال موفق")]
	Success = 1, 
	[Display(Name = "ارسال نا موفق")]
	Failed = 2,
}
public class SmsSendingStatus : BaseEnumEntity, IEntity<short>
{
}