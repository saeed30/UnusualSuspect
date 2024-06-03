using UnusualSuspect.Services.JcoSecurity;
using Microsoft.AspNetCore.Mvc;


namespace UnusualSuspect.ViewComponents.Areas.Components.ViewComponents;


public class UserRoleListViewComponent : ViewComponent
{
    public readonly IAccessManagmentService accessManagmentService;
    public UserRoleListViewComponent(IAccessManagmentService _accessManagmentService)
    {
        accessManagmentService = _accessManagmentService;
    }

    public async Task<IViewComponentResult> InvokeAsync( string UserName)
    {
        var items = await accessManagmentService.UserRolesList( UserName);
        return View("~/Areas/Components/Views/UserRoleList.cshtml", items);
    }
}
