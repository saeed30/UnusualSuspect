using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Api.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Identity;

public class ApplicationUserManager : IApplicationUserManager
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUnitOfWork _uow;
    private readonly IdentityErrorDescriber _errors;
    private readonly ILookupNormalizer _keyNormalizer;
    private readonly ILogger<ApplicationUserManager> _logger;
    private readonly IOptions<IdentityOptions> _optionsAccessor;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IEnumerable<IPasswordValidator<ApplicationUser>> _passwordValidators;
    private readonly IServiceProvider _services;
    private readonly DbSet<ApplicationUser> _users;
    private readonly DbSet<Role> _roles;
    private readonly IEnumerable<IUserValidator<ApplicationUser>> _userValidators;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationRoleService _roleManager;

    public ApplicationUserManager(
        UserManager<ApplicationUser> userManager,
        IApplicationRoleService roleManager,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<ApplicationUserManager> logger,
        IHttpContextAccessor contextAccessor,
        IUnitOfWork uow)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(_optionsAccessor));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(_passwordHasher));
        _userValidators = userValidators ?? throw new ArgumentNullException(nameof(_userValidators));
        _passwordValidators = passwordValidators ?? throw new ArgumentNullException(nameof(_passwordValidators));
        _keyNormalizer = keyNormalizer ?? throw new ArgumentNullException(nameof(_keyNormalizer));
        _errors = errors ?? throw new ArgumentNullException(nameof(_errors));
        _services = services ?? throw new ArgumentNullException(nameof(_services));
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
        _users = uow.Set<ApplicationUser>();
        _roles = uow.Set<Role>();
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser adminUser, string name)
    {
        bool ExistRols = await _roleManager.RoleExistsAsync(name);
        if (ExistRols) return IdentityResult.Success;
        return await _userManager.AddToRoleAsync(adminUser, name);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string rolename)
    {
        bool ExistRols = await _roleManager.RoleExistsAsync(rolename);
        if (!ExistRols)
            return IdentityResult.Failed(new IdentityError() { Description = "this role not exist!!!", Code = "-1" });

        if (!await _userManager.IsInRoleAsync(user, rolename))
            return await _userManager.AddToRoleAsync(user, rolename);
        else
            return IdentityResult.Failed(new IdentityError() { Description = "System Canot Add This user to this Role", Code = "-1" });
    }

    public async Task<bool> UserInRole(ApplicationUser user, string rolename)
    {
        try
        {
            return await _userManager.IsInRoleAsync(user, rolename);
        }
        catch// (Exception e)
        {
            return false;
        }
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
    public async Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.AddPasswordAsync(user, password);
    }
    public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
    {
        return await _userManager.DeleteAsync(user);
    }

    public async Task<ApplicationUser?> FindByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<ApplicationUser?> FindByNameAsync(string name)
    {
        return await _userManager.FindByNameAsync(name);
    }
    public async Task SetFireBaseToken(ApplicationUser user, string token, CancellationToken cancellationToken)
    {
        user.FireBaseToken = token;
        await _uow.SaveChangesAsync();
    }

    public Task<IdentityResult> SetLockoutEnabledAsync(ApplicationUser adminUser, bool enabled)
    {
        return _userManager.SetLockoutEnabledAsync(adminUser, enabled);
    }

    public Task<IdentityResult> UpdateAsync(ApplicationUser user)
    {
        return _userManager.UpdateAsync(user);
    }

    public Task UpdateLastLoginDateAsync(ApplicationUser user)
    {
        user.Lastlogin = DateTime.Now;
        return UpdateAsync(user);
    }
    public async Task UpdateAppInfoAsync(RegisterMobileViewModel model, ApplicationUser currentUser)
    {
        await _userManager.UpdateAsync(currentUser);
    }

    public async Task<UserProfileViewModel> GetProfileAsync(int userId)
    {
        var User = await FindByIdAsync(userId.ToString());
        return new UserProfileViewModel()
        {

            FirstName = User.FirstName,
        };
    }
    public ApplicationUser DetailsUserWithPhoneNumber(string PhoneNumber)
    {
        return _users.FirstOrDefault(x => x.PhoneNumber == PhoneNumber || x.UserName == PhoneNumber);
    }

    public ApplicationUser FindByName(string username)
    {
        return _users.SingleOrDefault(x => x.UserName == username);
    }
    public async Task<ResultAction> EditPassword(ApplicationUser model)
    {
        if (model.OldPassword.IsNull())
            return new ResultAction()
            {
                Success = false,
                MessageList = "کلمه عبور فعلی را وارد نمایید !"
            };
        if (model.Password.IsNull())
            return new ResultAction()
            {
                MessageList = "کلمه عبور جدید را وارد نمایید !"
            };
        if (model.ConfirmPassword.IsNull())
            return new ResultAction()
            {
                Success = false,
                MessageList = "تکرار کلمه عبور جدید را وارد نمایید !"
            };
        if (model.ConfirmPassword != model.Password)
            return new ResultAction()
            {
                Success = false,
                MessageList = "کلمه عبور جدید و تکرار آن مطابقت ندارد !"
            };

        var currentUser = await _userManager.FindByNameAsync(model.UserName);
        if (currentUser == null)
            return new ResultAction()
            {
                Success = false,
                MessageList = "حساب کاربری یافت نشد!"
            };
        var result = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.Password);
        if (result.Succeeded)
            return new ResultAction()
            {
                Success = true,
                MessageList = "ویرایش کلمه عبور با موفقیت انجام شد!"
            };
        ResultAction failResult = new ResultAction()
        {
            Success = false,
            MessageList = ""
        };
        foreach (var error in result.Errors)
            failResult.MessageList += error.Description + ". ";
        return failResult;
    }

}
