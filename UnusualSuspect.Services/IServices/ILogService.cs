using UnusualSuspect.Entities.Models;
using UnusualSuspect.ViewModels.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UnusualSuspect.Services.IServices;

public interface ILogService
{
    bool AddLog(LogObject model);
    IQueryable<LogObject> CombinedAllLogObjectSearch(LogObjectSearchViewModel model);
    List<SelectListItem> DropDownListAdminUser();
    List<SelectListItem> DropDownListObjectType();
    LogObject DetailsLogObject(int? LogObjectId);
}