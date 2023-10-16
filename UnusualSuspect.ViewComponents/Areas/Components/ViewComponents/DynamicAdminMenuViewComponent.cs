
using UnusualSuspect.Services.JcoSecurity;
using UnusualSuspect.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var items = await customeMenuService.ReturnRoleMenus(User.Identity.Name, User.IsInRole("Admin"));
        if(items != null)
            foreach(var item in items)
            {
                item.ChildeCustomeMenus = customeMenuService.CustomMenuListWithParentId(item.Id);
            }
        return View(items);
    }
}
