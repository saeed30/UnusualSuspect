using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UnusualSuspect.Admin.Controllers;

[Authorize(Roles = "Admin ,AdminPanelUserRole")]
[PersianTitle("مدیریت اطلاعات پایه")]
public class BaseInfoController : Controller
{

    public readonly IBaseInfoService _IBaseInfoService;
    public readonly IUploadServise _IuploadServise;

    public BaseInfoController(IBaseInfoService iBaseInfoService, IUploadServise IUploadServise)
    {
        _IBaseInfoService = iBaseInfoService;
        _IuploadServise = IUploadServise;
    }

}