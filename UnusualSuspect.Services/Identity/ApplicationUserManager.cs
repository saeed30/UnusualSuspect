using System.Reflection.Metadata;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Api.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Identity;

public class ApplicationUserManager(UserManager<ApplicationUser> userManager,
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
  : IApplicationUserManager
{
    private readonly DbSet<ApplicationUser> users = uow.Set<ApplicationUser>();
    private readonly DbSet<Role> roles = uow.Set<Role>();
    private readonly DbSet<UnusualSuspect.Entities.Models.Document> documents = uow.Set<UnusualSuspect.Entities.Models.Document>();

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser adminUser, string name)
    {
        bool existRols = await roleManager.RoleExistsAsync(name);
        if (existRols) return IdentityResult.Success;
        return await userManager.AddToRoleAsync(adminUser, name);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string rolename)
    {
        bool existRols = await roleManager.RoleExistsAsync(rolename);
        if (!existRols)
            return IdentityResult.Failed(new IdentityError() { Description = "this role not exist!!!", Code = "-1" });

        if (!await userManager.IsInRoleAsync(user, rolename))
            return await userManager.AddToRoleAsync(user, rolename);
        else
            return IdentityResult.Failed(new IdentityError() { Description = "System Canot Add This user to this Role", Code = "-1" });
    }

    public async Task<bool> UserInRole(ApplicationUser user, string rolename)
    {
        try
        {
            return await userManager.IsInRoleAsync(user, rolename);
        }
        catch// (Exception e)
        {
            return false;
        }
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return await userManager.CreateAsync(user, password);
    }
    public async Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
    {
        return await userManager.AddPasswordAsync(user, password);
    }
    public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
    {
        return await userManager.DeleteAsync(user);
    }

    public async Task<ApplicationUser?> FindByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task<ApplicationUser?> FindByNameAsync(string name)
    {
        return await userManager.FindByNameAsync(name);
    }
    public async Task SetFireBaseToken(ApplicationUser user, string token, CancellationToken cancellationToken)
    {
        user.FireBaseToken = token;
        await uow.SaveChangesAsync();
    }

    public Task<IdentityResult> SetLockoutEnabledAsync(ApplicationUser adminUser, bool enabled)
    {
        return userManager.SetLockoutEnabledAsync(adminUser, enabled);
    }

    public Task<IdentityResult> UpdateAsync(ApplicationUser user)
    {
        return userManager.UpdateAsync(user);
    }

    public Task UpdateLastLoginDateAsync(ApplicationUser user)
    {
        user.Lastlogin = DateTime.Now;
        return UpdateAsync(user);
    }
    public async Task UpdateAppInfoAsync(RegisterMobileViewModel model, ApplicationUser currentUser)
    {
        await userManager.UpdateAsync(currentUser);
    }

    public async Task<UserProfileViewModel> GetProfileAsync(int userId)
    {
        var user = await FindByIdAsync(userId.ToString());
        return new UserProfileViewModel()
        {

            FirstName = user.FirstName,
        };
    }
    public ApplicationUser DetailsUserWithPhoneNumber(string phoneNumber)
    {
        return users.FirstOrDefault(x => x.PhoneNumber == phoneNumber || x.UserName == phoneNumber);
    }

    public ApplicationUser FindByName(string username)
    {
        return users.SingleOrDefault(x => x.UserName == username);
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

        var currentUser = await userManager.FindByNameAsync(model.UserName);
        if (currentUser == null)
            return new ResultAction()
            {
                Success = false,
                MessageList = "حساب کاربری یافت نشد!"
            };
        var result = await userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.Password);
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
