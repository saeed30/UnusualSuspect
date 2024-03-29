using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Api.User;
using Microsoft.AspNetCore.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Contracts.Identity;


public interface IApplicationUserManager
{
    Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser adminUser, string name);
    Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string rolename);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<IdentityResult> DeleteAsync(ApplicationUser user);
    Task<ApplicationUser?> FindByIdAsync(string id);
    Task<ApplicationUser?> FindByNameAsync(string name);
    Task<UserProfileViewModel> GetProfileAsync(int userId);
    Task SetFireBaseToken(ApplicationUser user, string token, CancellationToken cancellationToken);
    Task<IdentityResult> SetLockoutEnabledAsync(ApplicationUser adminUser, bool enabled);
    Task UpdateAppInfoAsync(RegisterMobileViewModel model, ApplicationUser currentUser);
    Task<IdentityResult> UpdateAsync(ApplicationUser user);
    Task UpdateLastLoginDateAsync(ApplicationUser user);
    Task<bool> UserInRole(ApplicationUser user, string rolename);
    ApplicationUser DetailsUserWithPhoneNumber(string PhoneNumber);
    ApplicationUser FindByName(string username);
    Task<ResultAction> EditPassword(ApplicationUser model);

}
