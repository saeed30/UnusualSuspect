using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UnusualSuspect.Admin.Controllers;

[Authorize(Roles = "Admin ,AdminPanelUserRole")]
[PersianTitle("مدیریت اطلاعات پایه")]
public class BaseInfoController(IBaseInfoService baseInfoService, IUploadServise uploadServise) : Controller
{

}