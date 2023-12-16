using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.JcoSecurity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace UnusualSuspect.Services.Contracts.Identity;

public interface IApplicationSignInService
{
    Task SignInAsync(ApplicationUser currentUser, bool v);
    Task<ApplicationUser> ValidateSecurityStampAsync(ClaimsPrincipal principal);
    Task<SignInResult> PasswordSignInAsync(LogOnModel model);
    Task SignOutAsync();
}
