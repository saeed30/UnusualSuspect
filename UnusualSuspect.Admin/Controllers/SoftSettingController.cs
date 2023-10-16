using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnusualSuspect.Admin.Controllers;

public class SoftSettingController : Controller
{
    public readonly ISoftSettingService _ISoftSettingService;
    public readonly IUploadServise _IuploadServise;
    string LanguageName;
    public SoftSettingController(ISoftSettingService iSoftSettingService, IUploadServise IUploadServise)
    {
        _ISoftSettingService = iSoftSettingService;
        _IuploadServise = IUploadServise;
    }

    [ServiceFilter(typeof(UserFilters))]
    [PersianTitle("تنظیمات کسب و کار")]
    public ActionResult EditeSoftSetting()
    {
        LanguageName = "fa-IR";// Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
        return View(_ISoftSettingService.GetSoftSetting());
    }

    [ServiceFilter(typeof(UserFilters))]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditeSoftSetting(SoftSetting model)
    {
        return Json(_ISoftSettingService.EditSoftSetting(model));
    }

}
