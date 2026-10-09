using UnusualSuspect.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace UnusualSuspect.ViewComponents.Areas.Components.ViewComponents;


public class DynamicAdminMenuViewComponent : ViewComponent
{
    public readonly ICustomeMenuService  customeMenuService;
    public DynamicAdminMenuViewComponent(ICustomeMenuService  _customeMenuService)
    {
        customeMenuService = _customeMenuService;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Cached, with children already loaded (one query instead of one per menu item). The list is shared
        // between requests, so the view must only read it.
        var items = await customeMenuService.ReturnRoleMenusWithChildrenAsync(User.Identity.Name, User.IsInRole("Admin"));
        return View(items);
    }
}
