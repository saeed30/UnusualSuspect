﻿using UnusualSuspect.Services.Services;
using UnusualSuspect.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

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
        // between requests, so it is only read here and in the view; the "current" state goes in ViewData.
        var items = await customeMenuService.ReturnRoleMenusWithChildrenAsync(User.Identity.Name, User.IsInRole("Admin"));

        var (activeItemId, activeParentId) = FindActiveItem(
            items,
            RouteData.Values["controller"]?.ToString(),
            RouteData.Values["action"]?.ToString(),
            Request.Path.Value,
            Request.QueryString.Value,
            Request.Query);
        ViewData["ActiveMenuItemId"] = activeItemId;
        ViewData["ActiveMenuParentId"] = activeParentId;

        return View(items);
    }

    /// <summary>
    /// Finds the menu entry that matches the current request and, when it is a child entry, its parent
    /// (so the parent group can be opened). Only one entry is chosen, by best match:
    /// 4 = the entry's link is exactly the current path + query,
    /// 3 = same controller and action, and the entry's "?key=value" parameter (if any) is present in the query string,
    /// 2 = same controller and action,
    /// 1 = same controller only (e.g. an Edit or Details page opened from the entry's list page).
    /// On equal scores the entry that comes first in the menu wins.
    /// </summary>
    private static (int? ItemId, int? ParentId) FindActiveItem(
        IEnumerable<CustomMenu>? menus, string? controller, string? action,
        string? path, string? queryString, IQueryCollection query)
    {
        if (menus == null || string.IsNullOrEmpty(controller))
            return (null, null);

        var requestUrl = NormalizeUrl(path + queryString);
        int bestScore = 0;
        int? bestItemId = null;
        int? bestParentId = null;

        void Consider(CustomMenu item, int? parentId)
        {
            var menuController = item.AmAction?.AMController?.Name;
            if (string.IsNullOrEmpty(menuController) || string.IsNullOrEmpty(item.AmAction?.Name))
                return;

            int score = 0;
            if (string.Equals(NormalizeUrl($"/{menuController}/{item.AmAction.Name}{item.Parameter}"), requestUrl, StringComparison.OrdinalIgnoreCase))
                score = 4;
            else if (string.Equals(menuController, controller, StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(item.AmAction.Name, action, StringComparison.OrdinalIgnoreCase))
                    score = ParameterMatchesQuery(item.Parameter, query) ? 3 : 2;
                else
                    score = 1;
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestItemId = item.Id;
                bestParentId = parentId;
            }
        }

        foreach (var menu in menus.OrderBy(m => m.PositionId))
        {
            if (menu.ChildeCustomeMenus == null || menu.ChildeCustomeMenus.Count == 0)
            {
                Consider(menu, null);
                continue;
            }
            foreach (var child in menu.ChildeCustomeMenus.OrderBy(m => m.PositionId))
                Consider(child, menu.Id);
        }

        return (bestItemId, bestParentId);
    }

    // True when the entry has no query-style parameter, or every key=value of "?a=1&b=2" is in the request.
    // Path-style parameters ("/5") are only matched by the exact-URL rule above.
    private static bool ParameterMatchesQuery(string? parameter, IQueryCollection query)
    {
        if (string.IsNullOrWhiteSpace(parameter))
            return true;
        if (!parameter.StartsWith('?'))
            return false;

        foreach (var pair in QueryHelpers.ParseQuery(parameter))
        {
            if (!query.TryGetValue(pair.Key, out var requestValues))
                return false;
            foreach (var expected in pair.Value)
                if (!requestValues.Any(v => string.Equals(v, expected, StringComparison.OrdinalIgnoreCase)))
                    return false;
        }
        return true;
    }

    private static string NormalizeUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return "/";
        var queryStart = url.IndexOf('?');
        var path = (queryStart >= 0 ? url[..queryStart] : url).TrimEnd('/');
        var rest = queryStart >= 0 ? url[queryStart..] : string.Empty;
        return (path.Length == 0 ? "/" : path) + rest;
    }
}
