using UnusualSuspect.Common;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.JcoSecurity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UnusualSuspect.Services.Services;

public interface ICustomeMenuService
{
    int ChangePositionCustomMenu(int secondCustomMenuId, int selectedCustomMenuId);
    List<CustomMenu> CombinedAllCustomMenuSearch(CustomMenuSearchViewModel model);
    Task<ResultAction> CreateCustomMenu(CustomMenu model);
    IQueryable<CustomMenu> CustomMenuList();
    List<SelectListItem> CustomMenuUsersList(string PreName);
    Task<ResultAction> DeleteCustomMenu(int CustomMenuId, string UserName);
    CustomMenu DetailsCustomMenu(long CustomMenuId);
    Task<ResultAction> EditCustomMenu(CustomMenu model);
    int? GetActionId(string ActionName);
    string GetActionName(int? aMActionId);
    List<SelectListItem> ParentCustomMenuUsersList(string PreName);
    Task<List<CustomMenu>> ReturnUserMenus(string Username, bool IsAdmin);
    ResultAction UpdateActionCustomeMenu();
      Task<List<CustomMenu>> ReturnRoleMenus(string Username, bool IsAdmin);
    List<CustomMenu> CustomMenuListWithParentId(int parentId);
}

public class CustomeMenuService : ICustomeMenuService
{
    private readonly ILogger<BaseInfoService> _logger;
    private readonly IUnitOfWork _uow;
    private readonly DbSet<CustomMenu> customMenus;
    private readonly DbSet<ApplicationUser>  Users;
    private readonly DbSet<AmAction> amActions;
    //private readonly DbSet<ApplicationUserRole> UserRoles;
    private readonly ILogService logService;
    private readonly IApplicationRoleService _applicationRoleManager;
    public CustomeMenuService(ILogger<BaseInfoService> logger, IUnitOfWork uow, ILogService _iLogService, IApplicationRoleService ApplicationRoleManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
        customMenus = uow.Set<CustomMenu>();
         Users = uow.Set<ApplicationUser>();
        //UserRoles = uow.Set<ApplicationUserRole>();
        amActions = uow.Set<AmAction>();
        logService = _iLogService;
        _applicationRoleManager = ApplicationRoleManager;
    }
    public IQueryable<CustomMenu> CustomMenuList()
    {
        return customMenus.OrderBy(x => x.ParentId).ThenBy(x => x.PositionId);
    }
    public List<CustomMenu> CustomMenuListWithParentId(int parentId)
    {
        return customMenus.Include(m=>m.AmAction).Include(m => m.AmAction.AMController).Where(p=>p.ParentId== parentId).OrderBy(x => x.ParentId).ThenBy(x => x.PositionId).ToList();
    }
    public List<CustomMenu> CombinedAllCustomMenuSearch(CustomMenuSearchViewModel model)
    {
        var Items = customMenus.Include("AmAction").Where(x => (model.ParentId == null || x.ParentId == model.ParentId)).AsQueryable();
        if (!string.IsNullOrEmpty(model.KeyWord))
        {
            var searchTerms = model.KeyWord.Split(' ');
            var term = searchTerms[0];
            var CustomMenuList2 = Items.Where(x =>
                      (x.Name ?? "").Contains(term)
                      || (x.Id.ToString().Contains(term)));
            foreach (var tempTerm in searchTerms.Where(x => !string.IsNullOrEmpty(x) && x != term))
            {
                CustomMenuList2 = CustomMenuList2.Union(Items.Where(x =>
                      (x.Name ?? "").Contains(term)
                      || (x.Id.ToString().Contains(term))));
            }
            Items = CustomMenuList2;
        }
        switch (model.SortTypeId)
        {
            case 1:
                {
                    Items = Items.OrderBy(x => x.ParentId).ThenBy(x => x.PositionId);
                    break;
                }
            case 2:
                {
                    Items = Items.OrderByDescending(x => x.ParentId).ThenByDescending(x => x.PositionId);
                    break;
                }
            default:
                {
                    Items = Items.OrderBy(x => x.ParentId).ThenBy(x => x.PositionId);
                    break;
                }
        }
        model.ResultCount = Items.Count();
        return Items.Skip((model.Step - 1) * model.PageSize).Take(model.PageSize).ToList();
    }


    public CustomMenu DetailsCustomMenu(long CustomMenuId)
    {
        return customMenus.FirstOrDefault(x => x.Id == CustomMenuId);
    }

    public async Task<ResultAction> CreateCustomMenu(CustomMenu model)
    {
        try
        {
            model.PositionId = customMenus.Count() != 0 ? customMenus.Max(x => x.PositionId) + 1 : 1;
            customMenus.Add(model);
            logService.AddLog(new LogObject()
            {
                NextValue = HelperCommon.ShallowCopyEntityToString<CustomMenu>(model),
                PerValue = null,
                ObjectTypeId = "CustomMenu",
                ObjectTypeName = "CustomMenu",
                Title = "Create CustomMenu",
                DateCreate = DateTime.Now,
                UserName = model.UserName
            });
            await _uow.SaveChangesAsync();
            return new ResultAction()
            {
                Success = true,
                Id = model.Id.ToString(),
                MessageList = "منو با موفقیت ثبت شد"
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = "در انجام عمل خطایی رخ داده است. " + HelperCommon.ReturnMessageException(e)
            };
        }

    }

    public async Task<ResultAction> EditCustomMenu(CustomMenu model)
    {
        var Item = DetailsCustomMenu(model.Id);
        try
        {
            Item.Name = model.Name;
            Item.AMActionId = model.AMActionId;
            Item.NotifName = model.NotifName;
            Item.Parameter = model.Parameter;
            Item.ActionAndControllerName = model.ActionAndControllerName;
            Item.ParentId = model.ParentId;
            if (!string.IsNullOrEmpty(model.PatchIcon))
                Item.PatchIcon = model.PatchIcon;
            Item.FontAweSomeIcon = model.FontAweSomeIcon;
            logService.AddLog(new LogObject()
            {
                NextValue = HelperCommon.ShallowCopyEntityToString<CustomMenu>(Item),
                PerValue = HelperCommon.ShallowCopyEntityToString<CustomMenu>(DetailsCustomMenu(model.Id)),
                ObjectTypeId = "CustomMenu",
                ObjectTypeName = "CustomMenu",
                Title = "Update CustomMenu",
                DateCreate = DateTime.Now,
                UserName = model.UserName
            });
            await _uow.SaveChangesAsync();
            return new ResultAction()
            {
                Success = true,
                Id = Item.Id.ToString(),
                MessageList = "ویرایش با موفقیت انجام شد"
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = $"در ویرایش آیتم خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }

    public string GetActionName(int? aMActionId)
    {
        var Item = amActions.FirstOrDefault(x => x.Id == aMActionId);
        return Item != null ? Item.Name + "," + Item.AMController?.Name : null;
    }
    public int? GetActionId(string ActionName)
    {
        if (ActionName != null && ActionName.Contains(","))
        {
            var TempSt = ActionName.Split(',');
            var Item = amActions.FirstOrDefault(x => x.Name == TempSt[0] && x.AMController.Name == TempSt[1]);
            return Item?.Id;
        }
        else
            return null;
    }

    public async Task<ResultAction> DeleteCustomMenu(int CustomMenuId, string UserName)
    {
        var Item = DetailsCustomMenu(CustomMenuId);
        try
        {
            customMenus.Remove(Item);
            logService.AddLog(new LogObject()
            {
                NextValue = null,
                PerValue = HelperCommon.ShallowCopyEntityToString<CustomMenu>(Item),
                ObjectTypeId = "CustomMenu",
                ObjectTypeName = "CustomMenu",
                Title = "Delete CustomMenu",
                DateCreate = DateTime.Now,
                UserName = UserName
            });
            await _uow.SaveChangesAsync();
            return new ResultAction()
            {
                Success = true,
                TitleResult = "موفقیت آمیز",
                MessageList = "حذف آیتم با موفقیت انجام شد"
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                TitleResult = "خطا",
                MessageList = "در حذف آیتم خطایی رخ داده است. " + HelperCommon.ReturnMessageException(e)
            };
        }
    }

    public List<SelectListItem> CustomMenuUsersList(string PreName)
    {
        List<SelectListItem> CustomMenuUsers = new List<SelectListItem>
        {
            new SelectListItem() { Text = PreName, Value = "" }
        };
        CustomMenuUsers.AddRange(customMenus.Where(x => x.ParentId == null).Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        }).ToList());
        return CustomMenuUsers;
    }

    public List<SelectListItem> ParentCustomMenuUsersList(string PreName)
    {
        List<SelectListItem> CustomMenuUsers = new List<SelectListItem>
        {
            new SelectListItem() { Text = PreName, Value = "" }
        };
        CustomMenuUsers.AddRange(customMenus.Where(x => x.ParentId == null).Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        }).ToList());
        return CustomMenuUsers;
    }
    public int ChangePositionCustomMenu(int secondCustomMenuId, int selectedCustomMenuId)
    {
        var CustomMenuOB = customMenus.FirstOrDefault(x => x.Id == selectedCustomMenuId);
        var SecondCustomMenuOB = customMenus.FirstOrDefault(x => x.Id == secondCustomMenuId);
        if (CustomMenuOB != null && SecondCustomMenuOB != null)
        {
            var Temp = CustomMenuOB.PositionId;
            CustomMenuOB.PositionId = SecondCustomMenuOB.PositionId;
            SecondCustomMenuOB.PositionId = Temp;
            _uow.SaveChanges();
            return 1;
        }
        else
            return 0;
    }
    public ResultAction UpdateActionCustomeMenu()
    {
        foreach (var item in customMenus.ToList())
        {
            item.AMActionId = GetActionId(item.ActionAndControllerName);
        }
        _uow.SaveChanges();
        return new ResultAction() { Success = true };
    }

    public async Task<List<CustomMenu>> ReturnUserMenus(string Username, bool IsAdmin)
    {
        return await customMenus.Where(x => x.ParentId == null && (IsAdmin || x.AmAction.ActionForUsers.Any(u => u.ApplicationUser.UserName == Username)
        || x.ChildeCustomeMenus.Any(u => u.AmAction.ActionForUsers.Any(u => u.ApplicationUser.UserName == Username)))).ToListAsync();
    }
    public async Task<List<CustomMenu>> ReturnRoleMenus(string Username, bool IsAdmin)
    {
        if (Username.IsNull())
            return null;
        var user = Users.Where(x => x.UserName == Username).FirstOrDefault();
        if (user == null)
            return null;
        var roles = _applicationRoleManager.GetUserRoles(user.Id);

          //  UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();

        //return db.CustomMenu.Where(x => x.AmAction.ActionForRole.Any(u => roles.Contains(u.RoleId))).OrderBy(x => x.PositionId).ToList();



        return await customMenus.Include("AmAction").Include("AmAction.AMController").Where(x => x.ParentId == null && (IsAdmin || x.AmAction.ActionForRoles.Any(u => roles.Contains(u.RoleId.Value))
        || x.ChildeCustomeMenus.Any(u => u.AmAction.ActionForRoles.Any(u => roles.Contains(u.RoleId.Value))))).ToListAsync();
    }
}
