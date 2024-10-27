using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Services;
using Microsoft.AspNetCore.Mvc;

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
        return View(_ISoftSettingService.GetSoftSetting(true));
    }

    [ServiceFilter(typeof(UserFilters))]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditeSoftSetting(SoftSetting model)
    {
        return Json(_ISoftSettingService.EditSoftSetting(model));
    }

}
