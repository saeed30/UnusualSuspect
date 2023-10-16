using System;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.JcoSecurity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.JcoSecurity;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.JcoSecurity;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnusualSuspect.Admin.Controllers;

[Authorize(Roles = "Admin")]
[PersianTitle("مدیریت منو ها")]
public class CustomMenuController : Controller
{
    public readonly ICustomeMenuService  customeMenuService;
    public readonly IAccessManagmentService    accessManagmentService;
    public readonly IUploadServise   uploadServise;
    public CustomMenuController(ICustomeMenuService  _customeMenuService,IUploadServise _uploadServise , IAccessManagmentService _accessManagmentService)
    {
        customeMenuService = _customeMenuService;
        uploadServise = _uploadServise;
        accessManagmentService = _accessManagmentService;
    }

    [ServiceFilter(typeof(UserFilters))]
    [PersianTitle("نمایش لیست منو ها")]
    public ActionResult ShowCustomMenus()
    {
        var SearchModel = new CustomMenuSearchViewModel();
        var Items = customeMenuService.CombinedAllCustomMenuSearch(SearchModel);
        ViewData["SearchModel"] = SearchModel;
        return View(Items);

    }

    public ActionResult SearchCustomMenu()
    {
        return PartialView(new CustomMenuSearchViewModel());
    }

    public ActionResult ResaultSearchInCustomMenus(CustomMenuSearchViewModel SearchModel)
    {
        var Items = customeMenuService.CombinedAllCustomMenuSearch(SearchModel);
        ViewData["SearchModel"] = SearchModel;
        return PartialView(Items);
    }

    [ServiceFilter(typeof(UserFilters))]
    [PersianTitle("ایجاد منو")]
    public ActionResult CreateEditCustomMenu(int? CustomMenuId)
    {
        ViewBag.CreateOrEdit = true;
        if (CustomMenuId == null)
        {
            ViewBag.TitlePage = "ثبت منو جدید";
            return PartialView("_CreateEditCustomMenu", new CustomMenu());
        }
        else
        {
            ViewBag.TitlePage = "ویرایش مشخصات منو";
            ViewBag.CreateOrEdit = false;
            var CustomMenuOB = customeMenuService.DetailsCustomMenu(CustomMenuId ?? -1);
            return PartialView("_CreateEditCustomMenu", CustomMenuOB);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<JsonResult> CreateEditCustomMenu(CustomMenu model)
    {
        try
        {
            IFormFile file = (IFormFile)Request.Form.Files["Image"];
            model.PatchIcon = uploadServise.SaveImageSharp(file, "MenuIcon", 32, 32);
            model.ActionAndControllerName = customeMenuService.GetActionName(model.AMActionId);
            model.UserName = User.Identity.Name;
            if (model.Id == 0)
            {
                return Json(await customeMenuService.CreateCustomMenu(model));
            }
            else
                return Json(await customeMenuService.EditCustomMenu(model));
        }
        catch (Exception e)
        {
            return Json(new ResultAction()
            {
                Success = false,
                MessageList = $"در انجام عمل خطایی رخ داده است" + (HelperCommon.ReturnMessageException(e))
            });
        }
    }

    [ServiceFilter(typeof(UserFilters))]
    [PersianTitle("حذف منو")]
    public async Task<ActionResult> DeleteCustomMenu(int Id)
    {
        return Json(await customeMenuService.DeleteCustomMenu(Id, User.Identity.Name));
    }

    [ServiceFilter(typeof(UserFilters))]
    [PersianTitle("جابجایی آیتم های منو")]
    public JsonResult ChangePositionCustomMenu(int SecondCustomMenuId, int SelectedCustomMenuId)
    {
        return Json(customeMenuService.ChangePositionCustomMenu(SecondCustomMenuId, SelectedCustomMenuId));
    }

    public ActionResult UpdateActionCustomeMenu()
    {
        return View(customeMenuService.UpdateActionCustomeMenu());
    }

    [HttpPost]
    public JsonResult RetrieveActions(int SoftSectionId)
    {
        return Json(accessManagmentService.AmActionsList(SoftSectionId, " - - - - - "));
    }

    [HttpPost]
    public JsonResult RetrieveParentMenus(int SoftSectionId)
    {
        return Json(customeMenuService.ParentCustomMenuUsersList(" - - - - - "));
    }
}
