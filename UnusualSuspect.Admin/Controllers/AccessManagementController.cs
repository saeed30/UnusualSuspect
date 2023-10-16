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
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.JcoSecurity;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.JcoSecurity;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace UnusualSuspect.Admin.Controllers;

[Authorize(Roles = "Admin")]
[PersianTitle("مدیریت دسترسی ها")]
public class AccessManagementController : Controller
{
    public readonly IUploadServise _IuploadServise;
    public readonly IAccessManagmentService accessManagmentService;
    public readonly IApplicationUserService applicationUserService;
    public readonly IApplicationRoleService applicationRoleService;

    public AccessManagementController(IUploadServise IUploadServise,
                                      IAccessManagmentService _accessManagmentService,
                                      IApplicationUserService _applicationUserService,
                                      IApplicationRoleService _applicationRoleService)
    {
        _IuploadServise = IUploadServise;
        accessManagmentService = _accessManagmentService;
        applicationUserService = _applicationUserService;
        applicationRoleService = _applicationRoleService;
    }



    #region مدیریت نقش ها


    [PersianTitle("مدیریت نقش ها")]
    [ServiceFilter(typeof(UserFilters))]
    public ActionResult ShowAllSoftwareRoles()
    {
        var SearchModel = new SoftwareRoleSearchViewModel();
        ViewBag.Title = "مدیریت نقش ها";

        ViewData["SearchModel"] = SearchModel;
        return View(SearchModel);
    }



    public IActionResult ShowAllSoftwareRoles_Read([DataSourceRequest] DataSourceRequest request, SoftwareRoleSearchViewModel SearchModel)
    {
        var Items = accessManagmentService.CombinedAllSoftwareRolessearch(SearchModel);
        var result = Items.OrderByDescending(x => x.Id).ToDataSourceResult(request);
        return Json(result);
    }



    [PersianTitle("افزودن نقش ها")]
    [ServiceFilter(typeof(UserFilters))]
    public ActionResult CreateEditRole(int? Id)
    {
        if (Id == null)
        {
            ViewBag.Title = "تعریف نقش جدید";
            return View( new Role());
        }
        else
        {
            ViewBag.Title = "ویرایش مشخصات نقش";
            var Item = accessManagmentService.DetailsSoftwareRole(Id ?? -1);
            return View(Item);
        }
    }



    [ValidateAntiForgeryToken]
    [ServiceFilter(typeof(UserFilters))]
    [HttpPost]
    public ActionResult CreateEditRole(Role model)
    {
        model.ActionList = !string.IsNullOrEmpty(model.ActionList) ? model.ActionList.Substring(0, model.ActionList.Length - 1) : "";
        if (model.Id == 0)
        {
            return Json(accessManagmentService.CreateSoftwareRole(model).Result);
        }
        else
        {
            return Json(accessManagmentService.EditSoftwareRole(model));
        }
    }



    [PersianTitle("حذف نقش")]
    public JsonResult DeleteSoftwareRole(int Id)
    {
        return Json(accessManagmentService.DeleteSoftwareRole(Id));
    }



    #endregion



    #region مدیریت کنترلرها


    [PersianTitle("مدیریت کنترلرها و اکشن ها")]
    [ServiceFilter(typeof(UserFilters))]
    public ActionResult ShowAllControllers()
    {
        var SearchModel = new ControllerSearchViewModel();
        ViewBag.Title = "مدیریت کنترلرها";

        ViewData["SearchModel"] = SearchModel;
        return View(SearchModel);
    }


    [HttpPost]
    public IActionResult ShowAllControllers_Read([DataSourceRequest] DataSourceRequest request, ControllerSearchViewModel SearchModel)
    {
        var Items = accessManagmentService.CombinedAllControllerssearch(SearchModel);
       // var rs = Items.ToList();
        var result = Items.ToDataSourceResult(request);
        return Json(result);
    }



    public ActionResult UpdateControllers()
    {
        accessManagmentService.GetAllActionsOfControllers(Helper.GetAllActionsOfController());
        return RedirectToAction("ShowAllControllers");
    }



    [PersianTitle("ویرایش کنترلر")]
    [ServiceFilter(typeof(UserFilters))]
    public ActionResult EditController(int ControllerId)
    {
        var Item = accessManagmentService.DetailsController(ControllerId);
        ViewBag.TitlePage = $"ویرایش کنترلر {Item.Name}";
        return PartialView("_EditController", Item);
    }



    [ValidateAntiForgeryToken]
    [ServiceFilter(typeof(UserFilters))]
    [HttpPost]
    public ActionResult EditController(AMController model)
    {
        return Json(accessManagmentService.EditController(model));
    }


    #endregion



    #region مدیریت اکشن ها


    [PersianTitle("مدیریت اکشن ها")]
    [ServiceFilter(typeof(UserFilters))]
    public ActionResult ShowAllctionsOfController(int ControllerId)
    {
        var ControllerOB = accessManagmentService.DetailsController(ControllerId);
        var SearchModel = new AMACtionSearchViewModel() { TitlePage = $"اکشن های کنترلر { ControllerOB.Name }", ControllerId = ControllerId };
        ViewBag.Title = "مدیریت اکشن ها";

        ViewData["SearchModel"] = SearchModel;
        return View(SearchModel);
    }



    public IActionResult ShowAllctionsOfController_Read([DataSourceRequest] DataSourceRequest request, AMACtionSearchViewModel SearchModel)
    {
        var Items = accessManagmentService.CombinedAllAMACtionssearch(SearchModel);
        var result = Items.OrderByDescending(x => x.Id).ToDataSourceResult(request);
        return Json(result);
    }



    [PersianTitle("ویرایش اکشن")]
    [ServiceFilter(typeof(UserFilters))]
    public ActionResult EditActionOfController(int ActionId)
    {
        var Item = accessManagmentService.DetailsActionController(ActionId);
        ViewBag.TitlePage = $"ویرایش اکشن {Item.Name}";
        return PartialView("_EditActionController", Item);

    }



    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<ActionResult> EditActionOfController(AmAction model)
    {
        return Json(await accessManagmentService.EditActionController(model));
    }



    public JsonResult DeleteActionController(int Id)
    {
        return Json(accessManagmentService.DeleteActionController(Id));
    }


    #endregion





    [PersianTitle("لیست اکشن های کنترلر ها")]
    public ActionResult GetActionList(int SoftSectionId, int SoftwarerRoleId)
    {
        return View(accessManagmentService.ActionsList(SoftSectionId, SoftwarerRoleId).Result);
    }


    [PersianTitle("دسترسی دادن به کابران")]
    public ActionResult EditAccess(string UserName, int SoftSectionId, string rtnBackUrl)
    {
        ViewBag.rtnBackUrl = rtnBackUrl;
        ViewBag.TitlePage = "تعیین دسترسی";
        var Item = applicationUserService.DetailsUser(UserName);
        Item.SoftSectionId = SoftSectionId;
        //if (Item.ActionForUsers.Count > 0)
        //    Item.AccessTypeId = 2;
        if (applicationRoleService.GetUserRoles(Item.Id).Count > 0)
            Item.AccessTypeId = 1;
        return View(Item);
    }


    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult EditAccess(ApplicationUser model)
    {
        try
        {
            model.ActionList = !string.IsNullOrEmpty(model.ActionList) ? model.ActionList : "";
            model.SoftwarerRoleList = !string.IsNullOrEmpty(model.SoftwarerRoleList) ? model.SoftwarerRoleList : "";
            var Result = accessManagmentService.EditSoftwareRole(model).Result;
            switch (model.SoftSectionId)
            {
                case 1:
                    {
                        Result.Url = Url.Action("RegisterAdminPanelUser", "AdminPanleUser")?? "/AdminPanleUser/RegisterAdminPanelUser";
                        return Json(Result);
                    }
                default:
                    {
                        Result.Url = Url.Action("RegisterAdminPanelUser", "AdminPanleUser") ?? "/AdminPanleUser/RegisterAdminPanelUser";
                        return Json(Result);
                    }
            }


        }
        catch (Exception e)
        {
            return Json(new ResultAction
            {
                Success = true,
                MessageList = "در انجام عملیات خطایی رخ داده است. " + HelperCommon.ReturnMessageException(e)
            });
        }

    }


    [PersianTitle("اکشن های کاربران")]
    public ActionResult GetUserActionList(int SoftSectionId, string UserName)
    {
        return View(accessManagmentService.UserActionsList(SoftSectionId, UserName).Result);
    }

    public ActionResult GetUserRoleList(int SoftSectionId, string UserName)
    {
        return View(accessManagmentService.UserRolesList(  UserName).Result);
    }



}