using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Contracts.Identity;

public interface IIdentityDbInitializer
{
    void Initialize();
    void SeedData();
    Task<IdentityResult> SeedDatabaseWithAdminUserAsync();
}
