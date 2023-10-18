using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.Common.Enums
{
	public enum SmsMessageTextEnum
	{
		[Display(Name = "کد تایید جهت ورود به سامانه : {0}")]
		LoginCodeSms,
	}
}
