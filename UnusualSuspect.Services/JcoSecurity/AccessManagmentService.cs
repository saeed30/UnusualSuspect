using UnusualSuspect.Common;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.JcoSecurity;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.JcoSecurity;
using UnusualSuspect.ViewModels.Models;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.JcoSecurity;

public interface IAccessManagmentService
{
    Task<List<AmAction>> ActionsList(int SoftSectionId, int CustomerRoleId);
    IQueryable<AmAction> ActionsOfControllerList(int controllerId);
    void AddCustomeRolesToUser(int RoleId, int UserId);
    List<SelectListItem> AmActionsList(int TypeId, string PreName);
    List<SelectListItem> AmNotificationActionsList(int TypeId, string PreName);
    List<SelectListItem> RolesList();
    IQueryable<AmAction> CombinedAllAMACtionssearch(AMACtionSearchViewModel model);
    IQueryable<AMController> CombinedAllControllerssearch(ControllerSearchViewModel model);
    IQueryable<Role> CombinedAllSoftwareRolessearch(SoftwareRoleSearchViewModel model);
    IQueryable<AMController> ControllersList();
    Task<ResultAction> CreateSoftwareRole(Role model);
    ResultAction DeleteActionController(int actionId);
    ResultAction DeleteSoftwareRole(int id);
    AmAction DetailsActionController(int actionId);
    AMController DetailsController(int controllerId);
    Role DetailsSoftwareRole(int RoleId);
    Task<ResultAction> EditActionController(AmAction model);
    ResultAction EditController(AMController model);
    Task<ResultAction> EditSoftwareRole(ApplicationUser model);
    ResultAction EditSoftwareRole(Role model);
    void GetAllActionsOfControllers(List<ActionOfControllerViewModel> controlleractionlist);
    List<SelectListItem> RetrieveAccessTypeList();
    List<SelectListItem> SoftSectionList(string PreName);
    List<SelectListItem> SoftSectionListForCreateRole();
    IQueryable<Role> SoftwareRoleList();
    Task<List<AmAction>> UserActionsList(int SoftSectionId, string UserName);
    Task<List<Role>> UserRolesList( string UserName);
}

public class AccessManagmentService : IAccessManagmentService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _uow;
    private readonly DbSet<AMAreaName> _AMAreaName;
    private readonly DbSet<AMController> _AMController;
    private readonly DbSet<AmAction> _AmAction;
    private readonly DbSet<SoftSection> _SoftSection;
    //private readonly DbSet<ApplicationUserRole> _ApplicationUserRoles;
    private readonly DbSet<ActionForUser> _ActionForUser;
    private readonly DbSet<ActionForSoftwareRole> _ActionForSoftwareRole;
    private readonly DbSet<ActionForRole> _ActionForRole;
    private readonly DbSet<SoftwarerRoleForUser> _SoftwarerRoleForUser;
    private readonly DbSet<ApplicationUser> _User;
    private readonly DbSet<Role> _Role;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationRoleService applicationRoleService;

    public AccessManagmentService(IUnitOfWork uow,ApplicationDbContext context, UserManager<ApplicationUser> userManager,

        IApplicationRoleService applicationRoleService)
    {

        _context = context ?? throw new ArgumentNullException(nameof(_context));
        _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
        _AMAreaName = uow.Set<AMAreaName>();
        _AMController = uow.Set<AMController>();
        _AmAction = uow.Set<AmAction>();
        _SoftSection = uow.Set<SoftSection>();
        _ActionForUser = uow.Set<ActionForUser>();
        _ActionForSoftwareRole = uow.Set<ActionForSoftwareRole>();
        _ActionForRole = uow.Set<ActionForRole>();
        _Role = uow.Set<Role>();
        //_ApplicationUserRoles = uow.Set<ApplicationUserRole>();
        _SoftwarerRoleForUser = uow.Set<SoftwarerRoleForUser>();
        _User = uow.Set<ApplicationUser>();
        _userManager = userManager;
        this.applicationRoleService = applicationRoleService;

    }
    public void GetAllActionsOfControllers(List<ActionOfControllerViewModel> controlleractionlist)
    {
        foreach (var A in controlleractionlist.GroupBy(u => u.Area))
        {
            AMAreaName AMAreaNameOB = _AMAreaName.FirstOrDefault(x => x.Name == A.Key);
            if (AMAreaNameOB == null)
            {

                AMAreaNameOB = new AMAreaName() { Name = A.Key };
                _AMAreaName.Add(AMAreaNameOB);
            }
            _uow.SaveChanges();
            foreach (var C in A.GroupBy(d => d.Controller))
            {
                AMController AMControllerOB = _AMController.FirstOrDefault(x => x.Name == C.Key && x.AMAreaName.Name == A.Key);
                if (AMControllerOB == null)
                {
                    AMControllerOB = new AMController()
                    {
                        EnglishName = C.Key,
                        Name = C.Key,
                        AMAreaNameId = AMAreaNameOB.Id,
                        FarsiName = C.FirstOrDefault().ControllerFarsiName,
                        SoftSectionId = 1
                    };
                    _AMController.Add(AMControllerOB);
                }
                else
                {
                    AMControllerOB.EnglishName = C.FirstOrDefault().ControllerEnglishName;
                }
                _uow.SaveChanges();
                foreach (var CA in C)
                {
                    var ActionItem = _AmAction.FirstOrDefault(x => x.Name == CA.Action && x.AMController.Name == C.Key && x.AMController.AMAreaName.Name == A.Key);
                    if (ActionItem == null)
                    {
                        AmAction AmAction = new AmAction()
                        {
                            Name = CA.Action,
                            ReturnTypeName = CA.ReturnType,
                            AMControllerId = AMControllerOB.Id,
                            EnglishName = CA.EnglishName,
                            FarsiName = CA.FarsiName,
                            SoftSectionId = _SoftSection.FirstOrDefault(x => x.AreaName == CA.Area).Id
                        };
                        _AmAction.Add(AmAction);
                    }
                    else
                    {
                        ActionItem.EnglishName = CA.EnglishName;
                    }
                }
                _uow.SaveChanges();

            }
            _uow.SaveChanges();

        }
    }

    public ResultAction EditSoftwareRole(Role model)
    {
        var SoftwareRoleItem = DetailsSoftwareRole(model.Id);
        try
        {
            SoftwareRoleItem.Name = model.Name;
            SoftwareRoleItem.Title = model.Title;
            SoftwareRoleItem.SoftSectionId = model.SoftSectionId;
            if (model.ActionList.Length != 0)
            {
                var AmActionIds = model.ActionList.Split(',');
                foreach (var ActionForSoftwareRoleOB in SoftwareRoleItem.ActionForRole.ToList())
                {
                    var TempId = ActionForSoftwareRoleOB.AmActionId.ToString();
                    if (!AmActionIds.Any(x => x == TempId))
                    {
                        //var UserIdsList = SoftwareRoleItem.ApplicationUserRole.Select(x => x.UserId).ToList();
                        //var TempActionForUsers = _ActionForUser.Where(x => x.AmActionId == ActionForSoftwareRoleOB.AmActionId && UserIdsList.Contains(x.UserId));
                        //_ActionForUser.RemoveRange(TempActionForUsers);
                        _ActionForRole.Remove(ActionForSoftwareRoleOB);
                    }
                }
                foreach (var AmActionId in AmActionIds)
                {
                    var TempId = int.Parse(AmActionId);
                    var T = SoftwareRoleItem.ActionForRole.FirstOrDefault(x => x.AmActionId == TempId);
                    //var AmActionOB = __AmAction.FirstOrDefault(x => x.Id == TempId);
                    if (T == null)
                    {
                        //var UserIdsList = SoftwareRoleItem.ApplicationUserRole.Select(x => x.UserId).ToList();
                        //foreach (var UserId in UserIdsList)
                        //{
                        //    _ActionForUser.Add(new ActionForUser()
                        //    {
                        //        UserId = UserId,
                        //        AmActionId = TempId
                        //    });
                        //}
                        _ActionForRole.Add(new ActionForRole()
                        {
                            RoleId = model.Id,
                            AmActionId = TempId
                        });
                    }
                }

            }
            _uow.SaveChanges();
            return new ResultAction()
            {
                Success = true,
                Id = model.Id.ToString(),
                MessageList = $"نقش انتخاب شده را موفقیت ویرایش شد"
            };


        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                Id = model.Id.ToString(),
                MessageList = $"در ویرایش نقش خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }

    public void AddCustomeRolesToUser(int RoleId, int UserId)
    {
        try
        {
            var roleItem = _Role.FirstOrDefault(x => x.Id == RoleId);
            if (roleItem != null)
            {
                //_ApplicationUserRoles.RemoveRange(_ApplicationUserRoles.Where(x => x.UserId == UserId));

                var usr = _User.First(m => m.Id == UserId);
                _userManager.RemoveFromRoleAsync(usr, roleItem.Name);

                _ActionForUser.RemoveRange(_ActionForUser.Where(x => x.UserId == UserId));
                _SoftwarerRoleForUser.Add(new SoftwarerRoleForUser()
                {
                    UserId = UserId,
                    SoftwareRoleId = RoleId
                });
                _ActionForUser.AddRange(roleItem.ActionForRole.Select(x => new ActionForUser()
                {
                    UserId = UserId,
                    AmActionId = x.AmActionId
                }).ToList());
                _uow.SaveChanges();
            }
        }
        catch
        {

        }
    }

    public async Task<ResultAction> EditSoftwareRole(ApplicationUser model)
    {
        var UsrOB = _userManager.FindByNameAsync(model.UserName).Result;

        try
        {
            if (model.AccessTypeId == 2)
            {
                if (model.ActionList.Length != 0)
                {
                    var ActionIds = model.ActionList.Split(',');
                    foreach (var ActionUserOB in _ActionForUser.Where(m=>m.UserId== UsrOB.Id).ToList())
                    {
                        var TempId = ActionUserOB.AmActionId.ToString();
                        if (!ActionIds.Any(x => x == TempId))
                        {
                            var ItemOB = _ActionForUser.FirstOrDefault(x => x.Id == ActionUserOB.Id);
                            _ActionForUser.Remove(ItemOB);
                        }
                    }
                    foreach (var ActionId in ActionIds.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        var TempId = int.Parse(ActionId);
                        var T = _ActionForUser.Where(m => m.UserId == UsrOB.Id).ToList().FirstOrDefault(x => x.AmActionId == TempId);
                        var AmActionOB = _AmAction.FirstOrDefault(x => x.Id == TempId);
                        if (T == null)
                        {
                            _ActionForUser.Add(new ActionForUser()
                            {
                                UserId = model.Id,
                                AmActionId = TempId
                            });
                        }
                    }

                }
                else
                {
                    foreach (var ActionUserOB in _ActionForUser.Where(m => m.UserId == UsrOB.Id).ToList())
                    {
                        var ItemOB = _ActionForUser.FirstOrDefault(x => x.Id == ActionUserOB.Id);
                        _ActionForUser.Remove(ItemOB);
                    }
                }
                _uow.SaveChanges();
                return new ResultAction()
                {
                    Success = true,
                    Id = model.Id.ToString(),
                    MessageList = $"نقش انتخاب شده را موفقیت ویرایش شد"
                };
            }
            else if (model.AccessTypeId == 1)
            {
                _SoftwarerRoleForUser.RemoveRange(_SoftwarerRoleForUser.Where(x => x.UserId == UsrOB.Id).ToList());
                _ActionForUser.RemoveRange(_ActionForUser.Where(x => x.UserId == UsrOB.Id).ToList());

                var roles = applicationRoleService.GetUserRoleNames(UsrOB.UserName);
                await _userManager.RemoveFromRolesAsync(UsrOB, roles);
                //_ApplicationUserRoles.RemoveRange(_ApplicationUserRoles.Where(x => x.UserId == UsrOB.Id).ToList());
                _uow.SaveChanges();
                if (model.SoftwarerRoleList.Length != 0)
                {
                    var CustomerRoleIds = model.SoftwarerRoleList.Split(',');
                    foreach (var CustomerRoleId in CustomerRoleIds.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        var RoleId = int.Parse(CustomerRoleId);
                        var AmCustomerRoleOB = _Role.FirstOrDefault(x => x.Id == RoleId);
                        _SoftwarerRoleForUser.Add(new SoftwarerRoleForUser()
                        {
                            UserId = UsrOB.Id,
                            SoftwareRoleId = RoleId
                        });

                        await _userManager.AddToRoleAsync(UsrOB, AmCustomerRoleOB.Name);
                        //_ApplicationUserRoles.Add(new ApplicationUserRole()
                        //{
                        //    UserId = UsrOB.Id,
                        //    RoleId = RoleId
                        //});
                        _ActionForUser.AddRange(AmCustomerRoleOB.ActionForRole.Select(x => new ActionForUser()
                        {
                            UserId = UsrOB.Id,
                            AmActionId = x.AmActionId
                        }).ToList());

                    }
                }
                _uow.SaveChanges();
                return new ResultAction()
                {
                    Success = true,
                    Id = model.Id.ToString(),
                    MessageList = $"نقش انتخاب شده را موفقیت ویرایش شد"
                };
            }
            return new ResultAction()
            {
                Success = false,
                Id = model.Id.ToString(),
                MessageList = $"در ویرایش نقش خطایی رخ داده است"
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                Id = model.Id.ToString(),
                MessageList = $"در ویرایش نقش خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }


    public IQueryable<AMController> ControllersList()
    {
        return _AMController.OrderBy(x => x.AMAreaName.Id);
    }

    public IQueryable<Role> CombinedAllSoftwareRolessearch(SoftwareRoleSearchViewModel model)
    {
        return _Role.Select(x => new Role()
        {
            Id = x.Id,
            Name = x.Name,
            Title = x.Title,
            SoftSection = new SoftSection() { TypeName = x.SoftSection.TypeName }
        });
    }



    public IQueryable<AMController> CombinedAllControllerssearch(ControllerSearchViewModel model)
    {
        return _AMController.Select(x => new AMController()
        {
            Id = x.Id,
            Name = x.Name,
            FarsiName = x.FarsiName,
        });
    }



    public IQueryable<AmAction> CombinedAllAMACtionssearch(AMACtionSearchViewModel model)
    {
        return _AmAction.Where(x => x.AMControllerId == model.ControllerId).Select(x => new AmAction()
        {
            Id = x.Id,
            Name = x.Name,
            FarsiName = x.FarsiName,
            SoftSection = new SoftSection()
            {
                TypeName = x.SoftSection.TypeName,
            }
        });

    }
    public async Task<ResultAction> CreateSoftwareRole(Role model)
    {
        try
        {
            model.NormalizedName = model.Name.ToUpper();
            _Role.Add(model);
            await _uow.SaveChangesAsync();
            if (model.ActionList.Length != 0)
            {
                var ActionIds = model.ActionList.Split(',');

                foreach (var ActionId in ActionIds)
                {
                    var TempId = int.Parse(ActionId);
                    //var AmActionOB = __AmAction.FirstOrDefault(x => x.Id == TempId);
                    _ActionForRole.Add(new ActionForRole()
                    {
                        RoleId = model.Id,
                        AmActionId = TempId
                    });
                }
            }
            await _uow.SaveChangesAsync();
            return new ResultAction()
            {
                Success = true,
                Id = model.Id.ToString(),
                MessageList = $"نقش {model.Name} با موفقیت ایجاد شده"
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = $"در ایجاد نقش خطایی رخ داده است.  {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }

    public ResultAction EditController(AMController model)
    {
        var Item = DetailsController(model.Id);
        try
        {
            Item.FarsiName = model.FarsiName;
            Item.SoftSectionId = model.SoftSectionId;
            _uow.SaveChanges();
            return new ResultAction()
            {
                Success = true,
                Id = Item.Id.ToString(),
                MessageList = "تغییرات با موفقیت ثبت شدند"
            };

        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = $"در انجام عملیات خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }

    public AMController DetailsController(int controllerId)
    {
        return _AMController.FirstOrDefault(x => x.Id == controllerId);
    }
    public async Task<List<AmAction>> ActionsList(int SoftSectionId, int CustomerRoleId)
    {
        var Items = await _AmAction.Include(a=>a.AMController).ToListAsync();
        foreach (var item in Items)
        {
            item.Selected = _ActionForRole.Any(x => x.RoleId == CustomerRoleId && x.AmActionId == item.Id);
        }
        return Items;
    }

    public async Task<List<AmAction>> UserActionsList(int SoftSectionId, string UserName)
    {
        var Items = await _AmAction.Include(x=>x.AMController).Where(x => x.SoftSectionId == SoftSectionId).ToListAsync();
        var UserOb = _User.First(m => m.UserName == UserName);
        Items.ForEach(x =>  x.Selected = _ActionForUser.Any(m => m.UserId == UserOb.Id && m.AmActionId==x.Id ));
        return Items;
    }

    public async Task<List<Role>> UserRolesList(string UserName)
    {
       

        var Items = await _Role.ToListAsync();
        if (UserName!= null)
        {
            var UserOB = _User.FirstOrDefault(u => u.UserName == UserName);
            var UserRoles = _context.UserRoles.Where(x => x.UserId == UserOB.Id).Select(x => x.RoleId).ToList();
            Items.ForEach(x => { x.Selected = UserRoles.Any(d => d == x.Id); });

        }

        return Items;
    }


    public IQueryable<AmAction> ActionsOfControllerList(int controllerId)
    {
        return _AmAction.Where(x => x.AMControllerId == controllerId);
    }

    public Role DetailsSoftwareRole(int RoleId)
    {
        return _Role.Include(m=>m.ActionForRole).FirstOrDefault(x => x.Id == RoleId);
    }


    public IQueryable<Role> SoftwareRoleList()
    {
        return _Role;
    }

    public ResultAction DeleteSoftwareRole(int id)
    { 

        int[] RolesId = {1,2,3,5,12 };//necessary Roles
        List<int> authorsRange = new List<int>(RolesId);

        if (!authorsRange.Contains(id))
        {
            var Item = DetailsSoftwareRole(id);
            try
            {
                _Role.Remove(Item);
                _uow.SaveChanges();
                return new ResultAction()
                {
                    Success = true,
                    TitleResult = "موفقیت آمیز",
                    MessageList = "نقش انتخاب شده با موفقیت حذف گردید"
                };
            }
            catch (Exception e)
            {
                return new ResultAction()
                {
                    Success = false,
                    TitleResult = "خطا",
                    MessageList = $"در انجام عمل خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
                };
            }
            
        }
        else
        {
            return new ResultAction()
            {
                Success = false,
                TitleResult = "خطا",
                MessageList = $"نقش مورد نظر قابل حذف نمی باشد"
            };
        }

        
    }

    public async Task<ResultAction> EditActionController(AmAction model)
    {
        var Item = DetailsActionController(model.Id);
        try
        {
            Item.FarsiName = model.FarsiName;
            Item.SoftSectionId = model.SoftSectionId;
            await _uow.SaveChangesAsync();
            return new ResultAction()
            {
                Success = true,
                Id = Item.Id.ToString(),
                MessageList = "ویرایش آیتم با موفقیت انجام شد"
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                MessageList = HelperCommon.ReturnMessageException(e)
            };
        }
    }

    public AmAction DetailsActionController(int actionId)
    {
        return _AmAction.FirstOrDefault(x => x.Id == actionId);
    }

    public ResultAction DeleteActionController(int actionId)
    {
        var Item = DetailsActionController(actionId);
        try
        {
            _AmAction.Remove(Item);
            _uow.SaveChanges();
            return new ResultAction()
            {
                Success = true,
                TitleResult = "موفقیت آمیز",
                MessageList = "حذف اکشن با موفقیت انجام گردید"
            };
        }
        catch (Exception e)
        {
            return new ResultAction()
            {
                Success = false,
                TitleResult = "خطا",
                Id = Item.Id.ToString(),
                MessageList = $"در حذف اکشن خطایی رخ داده است. {HelperCommon.ReturnMessageException(e)}"
            };
        }
    }
    public List<SelectListItem> SoftSectionList(string PreName)
    {
        List<SelectListItem> SoftSectionList = new List<SelectListItem>
        {
            new SelectListItem() { Text = PreName, Value = "" }
        };
        SoftSectionList.AddRange(_SoftSection.Select(u => new SelectListItem
        {
            Text = u.TypeName,
            Value = u.Id.ToString()
        }).ToList());
        return SoftSectionList;
    }

    public List<SelectListItem> SoftSectionListForCreateRole()
    {
        List<SelectListItem> SoftSectionList = new List<SelectListItem>
        {
            new SelectListItem() { Text = "انتخاب بخش", Value = "" }
        };
        SoftSectionList.AddRange(_SoftSection.Where(x => x.Id < 4).Select(u => new SelectListItem
        {
            Text = u.TypeName,
            Value = u.Id.ToString()
        }).ToList());
        return SoftSectionList;
    }

    public List<SelectListItem> RetrieveAccessTypeList()
    {
        List<SelectListItem> AccessTypeList = new List<SelectListItem>
        {
            new SelectListItem() { Text = "نوع دسترسی را انتخاب کنید", Value = "" },
            new SelectListItem() { Text = "نقش", Value = "1" },
            new SelectListItem() { Text = "انفرادی", Value = "2" }
        };

        return AccessTypeList;
    }

    public List<SelectListItem> AmActionsList(int TypeId, string PreName)
    {
        List<SelectListItem> AmActions = new List<SelectListItem>
        {
            new SelectListItem() { Text = PreName, Value = "" }
        };
        AmActions.AddRange(_AmAction.Where(x => x.SoftSectionId == TypeId && x.ReturnTypeName != "string").Select(u => new SelectListItem
        {
            Text = string.IsNullOrEmpty(u.FarsiName) ? u.Name : u.FarsiName,
            Value = u.Id.ToString()
        }).ToList());
        return AmActions;
    }
    public List<SelectListItem> RolesList()
    {
        List<SelectListItem> ApplicationRoles = new List<SelectListItem>
        {
            new SelectListItem() { Text = "انتخاب نقش ", Value = "" }
        };
        ApplicationRoles.AddRange(_Role.Select(u => new SelectListItem
        {
            Text = $"{u.Name} - {u.Title}",
            Value = u.Id.ToString()
        }).ToList());
        return ApplicationRoles;
    }

    public List<SelectListItem> AmNotificationActionsList(int TypeId, string PreName)
    {
        List<SelectListItem> AmActions = new List<SelectListItem>
        {
            new SelectListItem() { Text = PreName, Value = "" }
        };
        AmActions.AddRange(_AmAction.Where(x => x.SoftSectionId == TypeId && x.ReturnTypeName == "string").Select(u => new SelectListItem
        {
            Text = u.EnglishName,
            Value = u.Id.ToString()
        }).ToList());
        return AmActions;
    }
}
