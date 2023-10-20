using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnusualSuspect.DataLayer;

namespace UnusualSuspect.Admin.Controllers;

public class ApplicationUserController : Controller
{
	public readonly IApplicationUserService _IApplicationUserService;
	public readonly IUploadServise _IuploadServise;
	private readonly IUnitOfWork _uow;

	public ApplicationUserController(IApplicationUserService IApplicationUserService, IUploadServise IUploadServise, IUnitOfWork uow)
	{
		_IApplicationUserService = IApplicationUserService;
		_IuploadServise = IUploadServise;
		_uow = uow;
	}



	[PersianTitle("مدیریت کاربران")]
	[ServiceFilter(typeof(UserFilters))]
	public IActionResult RegisterApplicationUser()
	{
		ViewBag.Title = "مدیریت کاربران  ";
		var SearchModel = new CustomerSearchViewModel();
		ViewData["SearchModel"] = SearchModel;

		return View(SearchModel);
	}



	public IActionResult RegisterApplicationUser_read([DataSourceRequest] DataSourceRequest request, string KeyWord, int Id)
	{
		var searchModel = new CustomerSearchViewModel();
		searchModel.Id = Id;
		searchModel.KeyWord = KeyWord;

		var Items = _IApplicationUserService.CombinedAllApplicationUserSearch(searchModel);
		var result = Items.OrderByDescending(x => x.UserId).ToDataSourceResult(request);
		return Json(result);
	}




	[PersianTitle("ثبت کاربر جدید")]
	[ServiceFilter(typeof(UserFilters))]
	public ActionResult CreateCustomerUser(int? UserId)
	{
		ViewBag.Create = true;

		if (UserId == null)
		{
			ViewBag.Title = "ایجاد کاربر";
			return PartialView("CreateCustomerUser", new ApplicationUser());

		}
		else
		{
			ViewBag.Title = "ویرایش مشخصات کاربر";
			ViewBag.Edit = false;
			var CustomerUserOB = _IApplicationUserService.DetailsApplicationUser(UserId ?? -1);
			return PartialView("CreateCustomerUser", CustomerUserOB);
		}
	}



	[ValidateAntiForgeryToken]
	[ServiceFilter(typeof(UserFilters))]
	[HttpPost]
	public ActionResult CreateCustomerUser(ApplicationUser model)
	{
		model.SoftwarerRoleList = !string.IsNullOrEmpty(model.SoftwarerRoleList) ? model.SoftwarerRoleList : "";
		ResultAction result;
		if (model.Id == 0)
			result = _IApplicationUserService.CreateApplicationUser(model, false).Result;
		else
			result = _IApplicationUserService.EditApplicationUser(model, false).Result;
		
		_uow.SaveChanges();
		return Json(result);
	}



	//[PersianTitle("ثبت کاربر جدید")]
	//[ServiceFilter(typeof(UserFilters))]
	//public ActionResult CreateApplicationUser(int? ApplicationUserId)
	//{
	//    ViewBag.Create = true;

	//    if (ApplicationUserId == null)
	//    {
	//        ViewBag.Title = "ایجاد کاربر";
	//        return PartialView("CreateApplicationUser", new ApplicationUser());

	//    }
	//    else
	//    {
	//        ViewBag.Title = "ویرایش مشخصات کاربر";
	//        ViewBag.Edit = false;
	//        var ApplicationUserOB = _IApplicationUserService.DetailsApplicationUser(ApplicationUserId ?? -1);
	//        return PartialView("CreateApplicationUser", ApplicationUserOB);
	//    }
	//}



	//[ValidateAntiForgeryToken]
	//[ServiceFilter(typeof(UserFilters))]
	//[HttpPost]
	//public ActionResult CreateApplicationUser(ApplicationUser model)
	//{

	//    model.ApplicationUser.ImageFile = (IFormFile)Request.Form.Files["Image"];

	//    if (model.Id == 0)
	//    {
	//        return Json(_IApplicationUserService.CreateApplicationUser(model).Result);
	//    }
	//    else
	//    {
	//        return Json(_IApplicationUserService.EditApplicationUser(model).Result);
	//    }
	//}



	//[PersianTitle("حذف کاربر")]
	//[ServiceFilter(typeof(UserFilters))]
	//public IActionResult DeleteApplicationUser(int Id)
	//{
	//    return Json(_IApplicationUserService.DeleteApplicationUser(Id, User.Identity.Name).Result);
	//}
}
