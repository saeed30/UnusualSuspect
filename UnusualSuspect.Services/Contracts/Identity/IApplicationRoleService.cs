using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.JcoSecurity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Contracts.Identity;

public interface IApplicationRoleService
{
    Task<bool> RoleExistsAsync(string rolename);
    Task<Role> CreateAsyncAndReturnRole(Role role);
    Task<IdentityResult> CreateAsync(Role role);
    IQueryable<Role> GetRoles();
    Task<int> AddActionForRole(CustomRole customRole);
    Task<List<int>> GetUserRolse(int userid);
    Task<Role> FindByNameAsync(string roleName);
    List<int> GetUserRoles(int userid);
    List<string> GetUserRoleNames(string username);
    IQueryable<string> GetUsersInRole(string rolename);
    IQueryable<ApplicationUser> GetApplicationUsersInRole(string rolename);
}
