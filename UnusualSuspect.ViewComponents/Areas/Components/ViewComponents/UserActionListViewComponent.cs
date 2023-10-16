using UnusualSuspect.Services.JcoSecurity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace UnusualSuspect.ViewComponents.Areas.Components.ViewComponents;


public class UserActionListViewComponent : ViewComponent
{
    public readonly IAccessManagmentService accessManagmentService;
    public UserActionListViewComponent(IAccessManagmentService _accessManagmentService)
    {
        accessManagmentService = _accessManagmentService;
    }
    public async Task<IViewComponentResult> InvokeAsync(int SoftSectionId, string UserName)
    {
        var items = await accessManagmentService.UserActionsList(SoftSectionId, UserName);
        return View("~/Areas/Components/Views/UserActionList.cshtml", items);
    }
}
