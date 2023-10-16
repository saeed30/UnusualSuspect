using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.JcoSecurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UnusualSuspect.ViewModels.Settings;
using UnusualSuspect.Common.Utilities;

namespace UnusualSuspect.Services.Identity;

public class ApplicationSignInManager : IApplicationSignInService
{
    private readonly IApplicationUserManager _userManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
    private readonly IOptions<IdentityOptions> _optionsAccessor;
    private readonly ILogger<ApplicationSignInManager> _logger;
    private readonly IAuthenticationSchemeProvider _schemes;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ApplicationSignInManager(
     IApplicationUserManager userManager,
      SignInManager<ApplicationUser> signInManager,
     IHttpContextAccessor contextAccessor,
     IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
     IOptions<IdentityOptions> optionsAccessor,
     ILogger<ApplicationSignInManager> logger,
     IAuthenticationSchemeProvider schemes,
     IUserConfirmation<ApplicationUser> confirmation)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(_userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        _claimsFactory = claimsFactory ?? throw new ArgumentNullException(nameof(_claimsFactory));
        _optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(_optionsAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        _schemes = schemes ?? throw new ArgumentNullException(nameof(_schemes));
    }
    public Task SignInAsync(ApplicationUser user, bool isPersistent)
    {
        return _signInManager.SignInAsync(user, isPersistent);
    }

    public async Task<SignInResult> PasswordSignInAsync(LogOnModel model)
    {
        return await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
    }

    public Task<ApplicationUser> ValidateSecurityStampAsync(ClaimsPrincipal principal)
    {
        return _signInManager.ValidateSecurityStampAsync(principal);
    }

    public Task SignOutAsync()
    {
        return _signInManager.SignOutAsync();
    }

}
