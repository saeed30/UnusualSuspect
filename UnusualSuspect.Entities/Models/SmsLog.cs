using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.Models
{
	public class SmsLog : BaseEntity
	{
		public SmsLog()
		{
			IsSend = false;
			IsActive = true;
			IsReadForSending = false;
			SendAttemptCount = 0;
		}
		[StringLength(64)]
		[Display(Name = "شماره تلفن")]
		public string PhoneNumber { get; set; }
		[StringLength(512)]
		[Display(Name = "محتوای پیامک")]
		public string MessageContent { get; set; }
		[Display(Name = "زمان افزدن به صف ارسال")]
		public DateTime DateTimeAddedToQueue { get; set; }
		[Display(Name = "زمان آخرین تلاش جهت ارسال")]
		public DateTime? DateTimeSent { get; set; }
		[Display(Name = "وضعیت ارسال")]
		public short? SmsSendingStatusId { get; set; }
		[ForeignKey("SmsSendingStatusId")]
		public SmsSendingStatus? SmsSendingStatus { get; set; }
		[StringLength(1024)]
		[Display(Name = "توضیحات وضعیت ارسال")]
		public string? StatusMessage { get; set; }
		[Display(Name = "شناسه ارسال")]
		public long? Identifier { get; set; }
		[Display(Name = "خطای ارسال")]
		public string? SendingError { get; set; }
		[Display(Name = "ارسال شده است؟")]
		public bool IsSend { get; set; }
		[Display(Name = "فعال")]
		public bool IsActive { get; set; }
		[Display(Name = "جهت ارسال خوانده شده است")]
		public bool IsReadForSending { get; set; }
		[Display(Name = "تعداد تلاش جهت ارسال")]
		public short SendAttemptCount { get; set; }
	}
}
