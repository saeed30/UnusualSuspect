using UnusualSuspect.Entities.JcoSecurity;
using UnusualSuspect.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.Entities.Identity;

public class ApplicationUser : IdentityUser<int>, IEntity<int>
{
	public bool State { get; set; }
	public string? MobileToken { get; set; }
	public DateTime? ExpireToken { get; set; }
	public DateTime? Lastlogin { get; set; }
	public string? FireBaseToken { get; set; }
	[Display(Name = "نام")]
	public string? FirstName { get; set; }
	[Display(Name = "نام خانوادگی")]
	public string? LastName { get; set; }


	public int? DocumentId { get; set; }
	[ForeignKey("DocumentId")]
	public virtual Document? Document { get; set; }

	public string? AppVersion { get; set; }
	public string? PatchImage { get; set; }
	public DateTime DateCreate { get; set; }

	public virtual ICollection<ActionForUser> ActionForUsers { set; get; }
	//public virtual ICollection<LogObject> LogObjects { set; get; }
	//public virtual ICollection<SoftwarerRoleForUser> SoftwarerRoleForUsers { set; get; }
	public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }
	public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; }
	//public virtual ICollection<IdentityUserToken<int>> Tokens { get; set; }



	public bool IsActive { set; get; }
	//public virtual ICollection<AdminPanleUser> AdminPanleUsers { set; get; }
	//public virtual ICollection<Customer> Customers { set; get; }

	public string? Token { get; set; }
	public string? PhoneNumberValidationCode { get; set; }
	public string? CodeForResetPassword { get; set; }
	public DateTime? SendCodeDate { set; get; }

	[NotMapped]
	public string FullName { get { return FirstName + " " + LastName; } }
	[NotMapped]
	public string? FileConfig { set; get; }
	[NotMapped, Display(Name = "رمز عبور"), DataType(DataType.Password)]
	public string? Password { set; get; }
	[NotMapped, Display(Name = "تکرار رمز عبور"), DataType(DataType.Password)]
	public string? ConfirmPassword { set; get; }
	[NotMapped, Display(Name = "نوع دسترسی")]
	public int? AccessTypeId { set; get; }
	[NotMapped]
	public int SoftSectionId { set; get; }

	[NotMapped]
	public string? ActionList { set; get; }

	[NotMapped]
	public string? SoftwarerRoleList { set; get; }
	[NotMapped]
	public IFormFile? ImageFile { set; get; }

	[NotMapped, Display(Name = "رمز عبور قدیمی"), DataType(DataType.Password)]
	public string? OldPassword { set; get; }
}
//public class ApplicationUserRole : IdentityUserRole<int>, IEntity
//{

//}
