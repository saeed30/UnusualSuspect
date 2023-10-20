using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnusualSuspect.DataLayer;

namespace UnusualSuspect.Admin.Controllers;

public class AccountController : Controller
{
	private readonly IApplicationUserManager _userManager;
	private readonly IApplicationSignInService _signInManager;
	private readonly ISoftSettingService _softSettingService;
	private readonly IApplicationUserService _applicationUserService;
	private readonly ISmsService _smsService;
	private readonly IUnitOfWork _uow;

	public AccountController(IApplicationSignInService signInManager,
															IApplicationUserManager userManager,
															IApplicationUserService applicationUserService,
															ISoftSettingService softSettingService,
															ISmsService smsService,
															IUnitOfWork uow)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_softSettingService = softSettingService;
		_smsService = smsService;
		_applicationUserService = applicationUserService;
		_uow = uow;
	}



	public IActionResult Login(string PhoneNumber, string returnUrl = null)
	{

		return View(new LoginViewModel() { ReturnUrl = returnUrl, PhoneNumber = PhoneNumber });
	}
	[AutoValidateAntiforgeryToken]
	[HttpPost]
	public async Task<IActionResult> Login(LoginViewModel model)
	{
		model.PhoneNumber = model.PhoneNumber.Fa2En().FixPersianChars();
		model.Password = model.Password.Fa2En().FixPersianChars();
		var UserOB = _userManager.DetailsUserWithPhoneNumber(model.PhoneNumber);
		string wrongUserOrPass = "نام کاربری یا کلمه عبور اشتباه است.";
		if (UserOB == null)
		{
			return Json(new ResultAction()
			{
				Success = false,
				MessageList = wrongUserOrPass
			});
		}
		var result = await _signInManager.PasswordSignInAsync(new ViewModels.JcoSecurity.LogOnModel()
		{
			UserName = model.PhoneNumber,
			Password = model.Password,
			RememberMe = false
		});
		if (result.Succeeded)
		{
			if (result.IsLockedOut)
			{
				await _signInManager.SignOutAsync();
				return Json(new ResultAction()
				{
					Success = false,
					MessageList = "حساب کاربری شما قفل شده است. لطفا با مدیر سامانه تماس بگیرید."
				});
			}
			else if (!UserOB.IsActive)
			{
				await _signInManager.SignOutAsync();
				return new JsonResult(new ResultAction
				{
					Success = false,
					MessageList = "حساب کاربری شما توسط مدیریت غیرفعال شده است. در صورت نیاز به فعال سازی از بخش تماس با ما درخواست تان را به ما ارسال نمائید"
				});
			}
			return new JsonResult(new ResultAction { Success = true, Url = model.ReturnUrl ?? Url.Action("Index", "Dashboard", new { area = "" }) });
		}
		return Json(new ResultAction()
		{
			Success = false,
			MessageList = wrongUserOrPass
		});
	}

	[AutoValidateAntiforgeryToken]
	[HttpPost]
	public async Task<IActionResult> SendCodeToPhoneNumber(LoginViewModel model)
	{
		var UserOB = _userManager.DetailsUserWithPhoneNumber(model.PhoneNumber.Fa2En().FixPersianChars());
		if (UserOB == null)
		{
			var RegisterResult = await _applicationUserService.CreateApplicationUser(new Entities.Identity.ApplicationUser()
			{
				UserName = model.PhoneNumber,
				PhoneNumber = model.PhoneNumber,
				Password = model.PhoneNumber,
				SoftwarerRoleList = ""
			}
			, true);
			if (RegisterResult.Success == false)
			{
				_uow.SaveChanges();
				return Json(RegisterResult);
			}
			else
				UserOB = _userManager.DetailsUserWithPhoneNumber(model.PhoneNumber.Fa2En().FixPersianChars());
		}
		var user = await _userManager.FindByIdAsync(UserOB.Id.ToString());
		user.PhoneNumberValidationCode = new Random().Next(10000, 99999).ToString();
		var SettingOB = _softSettingService.GetSoftSetting();
		if (_smsService.SendSmsAsync(user.PhoneNumber, $"کد {user.PhoneNumberValidationCode} جهت ورود به سامانه {SettingOB?.BussinessTitle} وارد مائید").Result)
		{
			user.SendCodeDate = DateTime.Now;
			if (user.SecurityStamp == null)
				user.SecurityStamp = Guid.NewGuid().ToString();
			await _userManager.UpdateAsync(user);
			return Json(new ResultAction()
			{
				Success = true,
				MessageList = $"رمز ارسال شده به شماره موبایل خود را در باکس زیر وارد نمائید. {user.PhoneNumberValidationCode}",
				Params1 = model.ReturnUrl,
				Params2 = model.PhoneNumber,
				Params3 = user.SendCodeDate.ToString()
			});
		}
		else
		{
			return Json(new ResultAction()
			{
				Success = false,
				MessageList = "در اراسال اس ام اس خطایی رخ داده است . لطفا بعد از چند دقیقه دیگر مجدد تست نمائید"
			});
		}
	}

	public async Task<IActionResult> ReSendCodeToPhoneNumber(string PhoneNumber)
	{

		var UserOB = _userManager.DetailsUserWithPhoneNumber(PhoneNumber.Fa2En().FixPersianChars());
		if (UserOB == null)
		{
			return Json(new ResultAction()
			{
				Success = false,
				MessageList = "کاربری با این مشخصات در سیستم وجود ندارد"
			});
		}
		else
		{
			var user = await _userManager.FindByIdAsync(UserOB.Id.ToString());
			if (user.SendCodeDate == null || DateTime.Now.AddMinutes(-2) > user.SendCodeDate)
			{
				var SettingOB = _softSettingService.GetSoftSetting();
				user.PhoneNumberValidationCode = new Random().Next(10000, 99999).ToString();
				if (_smsService.SendSmsAsync(user.PhoneNumber, $"کد {user.PhoneNumberValidationCode} جهت ورود به سامانه {SettingOB?.BussinessTitle} وارد مائید").Result)
				{
					user.SendCodeDate = DateTime.Now;
					await _userManager.UpdateAsync(user);
					return Json(new ResultAction()
					{
						Success = true,
						MessageList = user.PhoneNumberValidationCode,
						Params3 = user.SendCodeDate.ToString()
					});
				}
				else
				{
					return Json(new ResultAction()
					{
						Success = false,
						MessageList = "در اراسال اس ام اس خطایی رخ داده است . لطفا بعد از چند دقیقه دیگر مجدد تست نمائید"
					});
				}
			}
			else
			{

				return Json(new ResultAction()
				{
					Success = false,
					MessageList = "مهلت ارسال مجدد کد دو دقیقه بعد از ارسال موفق است. "
				});
			}
		}
	}
	[HttpGet]
	public IActionResult ConfirmLoginCode(string PhonNumber, string retUrl, string SendDate)
	{
		return PartialView(new LoginViewModel()
		{
			PhoneNumber = PhonNumber,
			ReturnUrl = retUrl,
			SendDate = DateTime.Parse(SendDate)
		});
	}

	[HttpGet]
	public IActionResult NewConfirmLoginCode(string PhonNumber, string retUrl, string SendDate)
	{
		return PartialView(new LoginViewModel()
		{
			PhoneNumber = PhonNumber,
			ReturnUrl = retUrl,
			SendDate = DateTime.Parse(SendDate)
		});
	}

	[AutoValidateAntiforgeryToken]
	[HttpPost]
	public async Task<IActionResult> NewConfirmSendedCode(string PhoneNumber, string PhoneNumberCode, string ReturnUrl)
	{
		var user = _userManager.DetailsUserWithPhoneNumber(PhoneNumber.Fa2En().FixPersianChars());
		if (user == null)
			return Json(new ResultAction()
			{
				Success = false,
				MessageList = "کاربری با این مشخصات در سیستم وجود ندارد"
			});
		else
		{
			if (user.PhoneNumberValidationCode != PhoneNumberCode)
				return Json(new ResultAction()
				{
					Success = false,
					MessageList = "کد وارد شده نادرست است"
				});
			else
			{
				if (user.PhoneNumberConfirmed == false)
				{
					user.PhoneNumberConfirmed = true;
					await _userManager.UpdateAsync(user);
				}
				await _signInManager.SignInAsync(user, true);
				//var user = await _userManager.FindByNameAsync(model.PhoneNumber.Fa2En().FixPersianChars());
				if (user.IsActive == false)
				{
					await _signInManager.SignOutAsync();
					return new JsonResult(new ResultAction
					{
						Success = false,
						MessageList = "حساب کاربری شما توسط مدیریت غیرفعال شده است. در صورت نیاز به فعال سازی از بخش تماس با ما درخواست تان را به ما ارسال نمائید"
					});
				}

				//if (User.IsInRole( "Admin") ||  User.IsInRole("AdminPanelUserRole"))
				return new JsonResult(new ResultAction { Success = true, Url = ReturnUrl ?? Url.Action("Index", "Dashboard", new { area = "" }) });
				//else
				//    return new JsonResult(new ResultAction { Success = false, MessageList = "دسترسی شما مجاز نمی باشد", Url = ReturnUrl ?? Url.Action("Index", "Request", new { area = "" }) });

			}
		}

	}

	public ActionResult LogOff(string BackUrl)
	{
		_signInManager.SignOutAsync();
		return Redirect(BackUrl ?? Url.Action("Login", "Account", new { area = "" }));
	}

	[PersianTitle(" ویرایش پروفایل")]
	[ServiceFilter(typeof(UserFilters))]
	public IActionResult EditProfile()
	{
		var UserOB = _applicationUserService.DetailsApplicationUserWhitUserName(User.Identity.Name);

		if (UserOB.Document != null)
		{
			string imageBase64Data = Convert.ToBase64String(UserOB.Document.File);
			UserOB.PatchImage = string.Format("data:image/png;base64,{0}", imageBase64Data);
			UserOB.FileConfig = "{ description: '', size: " + 5 + ", caption: '', url: '/Member/DeleteProfileDocument?USername=" + UserOB.UserName + "', key: '" + UserOB.DocumentId + "', downloadUrl: false}";

		}

		return View(UserOB);
	}



	[ValidateAntiForgeryToken]
	[HttpPost]
	public ActionResult EditProfile(ApplicationUser model)
	{
		model.ImageFile = (IFormFile)Request.Form.Files["Image"];
		var result = _applicationUserService.EditApplicationUser(model, true).Result;
		_uow.SaveChanges();
		return Json(result);
	}

}
