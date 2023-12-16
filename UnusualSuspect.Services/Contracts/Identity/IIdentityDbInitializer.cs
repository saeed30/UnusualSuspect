using Microsoft.AspNetCore.Identity;

namespace UnusualSuspect.Services.Contracts.Identity;

public interface IIdentityDbInitializer
{
    void Initialize();
    void SeedData();
    Task<IdentityResult> SeedDatabaseWithAdminUserAsync();
}
