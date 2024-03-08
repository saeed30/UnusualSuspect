using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace UnusualSuspect.Admin.Controllers;

public class HomeMenuController : Controller
{

    public readonly IHomeMenuService _IHomeMenuService;
    public readonly IUploadServise _IuploadServise;

    public HomeMenuController(IHomeMenuService iHomeMenuService, IUploadServise IUploadServise)
    {
        _IHomeMenuService = iHomeMenuService;
        _IuploadServise = IUploadServise;
    }



    [PersianTitle("لیست منو")]
    [ServiceFilter(typeof(UserFilters))]
    public IActionResult ShowMenuItem()
    {
        ViewBag.Title = "لیست منو";
        return View();
    }



    public IActionResult ShowMenuItem_read([DataSourceRequest] DataSourceRequest request)
    {
        var Items = _IHomeMenuService.ShowAll();
        var result = Items.OrderByDescending(x => x.Id).ToDataSourceResult(request);
        return Json(result);
    }



    [PersianTitle("ایجاد منو")]
    [ServiceFilter(typeof(UserFilters))]
    public ActionResult CreateEditMenuItem(int? homeMenuId)
    {
        if (homeMenuId == null)
        {
            ViewBag.Title = "ایجاد منو";
            return PartialView("CreateEditMenuItem", new HomeMenu());
        }
        else
        {
            ViewBag.Title = "ویرایش منو";
            var MenuOB = _IHomeMenuService.DetailsMenuItem(homeMenuId ?? -1);
            ViewBag.Title = $"ویرایش منو";
            return PartialView("CreateEditMenuItem", MenuOB);
        }
    }



    [ValidateAntiForgeryToken]
    [ServiceFilter(typeof(UserFilters))]
    [HttpPost]
    public ActionResult CreateEditMenuItem(HomeMenu model, int Step)
    {
        model.UserName = User.Identity.Name;
        ViewBag.Step = Step;
        model.ImageFile = (IFormFile)Request.Form.Files["Image"];

        if (model.Id == 0)
        {
            return Json(_IHomeMenuService.CreateItem(model));
        }
        else
        {
            return Json(_IHomeMenuService.EditItem(model));
        }
    }



    [PersianTitle("حذف آیتم")]
    [ServiceFilter(typeof(UserFilters))]
    public IActionResult DeleteMenuItem(int Id)
    {
        return Json(_IHomeMenuService.DeleteItem(Id, User.Identity.Name));
    }



    [ServiceFilter(typeof(UserFilters))]
    [PersianTitle("فعال کردن آیتم")]
    public JsonResult ActiveDeactiveMenuItem(int Id)
    {
        return Json(_IHomeMenuService.ActiveDeactiveMenuItem(Id));
    }



}
