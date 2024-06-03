using UnusualSuspect.Services.JcoSecurity;
using Microsoft.AspNetCore.Mvc;

namespace UnusualSuspect.ViewComponents.Areas.Components.ViewComponents;

public class ActionsListViewComponent : ViewComponent
{
    public readonly IAccessManagmentService accessManagmentService;
    public ActionsListViewComponent(IAccessManagmentService _accessManagmentService)
    {
        accessManagmentService = _accessManagmentService;
    }
    public async Task<IViewComponentResult> InvokeAsync(int SoftSectionId, int SoftwarerRoleId)
    {
        var items = await accessManagmentService.ActionsList(SoftSectionId, SoftwarerRoleId);
        return View("~/Areas/Components/Views/ActionsList.cshtml", items);
    }
}
