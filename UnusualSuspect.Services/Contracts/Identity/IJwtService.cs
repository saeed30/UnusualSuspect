using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Identity;

namespace UnusualSuspect.Services.Contracts.Identity;

public interface IJwtService
{
    Task<AccessToken> GenerateAsync(ApplicationUser user);
}
