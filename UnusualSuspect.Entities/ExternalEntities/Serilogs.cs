using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.ExternalEntities
{
	public class Serilogs
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "متن پیام")]
		public string? Message { get; set; }

		[Display(Name = "قالب پیام")]
		public string? MessageTemplate { get; set; }

		[Display(Name = "سطح لاگ")]
		public string? Level { get; set; }

		[Column("TimeStamp", TypeName = "DateTime")]
		[Display(Name = "زمان")]
		public DateTime? TimeStamp { get; set; }

		[Display(Name = "جزئیات خطا")]
		public string? Exception { get; set; }

		[Display(Name = "سایر اطلاعات")]
		public string? Properties { get; set; }

		[Display(Name = "نوع رویداد")]
		public int? EventTypeId { get; set; }

		[Display(Name = "کلید اصلی")]
		public int? DataKey { get; set; }

		[Display(Name = "اطلاعات اضافه")]
		public string? ExtraInfo { get; set; }

		[MaxLength(500)]
		[Display(Name = "نام کاربری")]
		public string? UserName { get; set; }

		[MaxLength(100)]
		[Display(Name = "آدرس IP")]
		public string? ClientIp { get; set; }

		[MaxLength(500)]
		[Display(Name = "رابط کاربری")]
		public string? UserAgent { get; set; }

		[MaxLength(100)]
		[Display(Name = "کد تابع")]
		public string? ActionId { get; set; }

		[Display(Name = "نام تابع")]
		public string? ActionName { get; set; }

		[MaxLength(100)]
		[Display(Name = "کد درخواست")]
		public string? RequestId { get; set; }

		[Display(Name = "آدرس")]
		public string? RequestPath { get; set; }
		[Display(Name = "SourceContext")]
		public string? SourceContext { get; set; }
		[NotMapped]
		public string ShamsiTimestamp => !TimeStamp.HasValue ? "" : TimeStamp.Value.ToString();
	}
}
