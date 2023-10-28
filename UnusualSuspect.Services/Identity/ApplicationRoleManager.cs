using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.JcoSecurity;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.JcoSecurity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Identity;

public class ApplicationRoleManager : IApplicationRoleService
{
    private readonly IHttpContextAccessor contextAccessor;
    private readonly IUnitOfWork uow;
    private readonly IdentityErrorDescriber errors;
    private readonly ILookupNormalizer keyNormalizer;
    private readonly ILogger<ApplicationRoleManager> logger;
    private readonly IOptions<IdentityOptions> optionsAccessor;
    private readonly IServiceProvider services;
    private readonly DbSet<Role> roles;
    private readonly DbSet<ActionForRole> actionForRole;
    private readonly RoleManager<Role> roleManager;
    private readonly ApplicationDbContext context;

    public ApplicationRoleManager(
        RoleManager<Role> roleManager,
        IOptions<IdentityOptions> optionsAccessor,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<ApplicationRoleManager> logger,
        IHttpContextAccessor contextAccessor,
        ApplicationDbContext context,
        IUnitOfWork uow)
    {
        this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        this.optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(optionsAccessor));
        this.keyNormalizer = keyNormalizer ?? throw new ArgumentNullException(nameof(keyNormalizer));
        this.errors = errors ?? throw new ArgumentNullException(nameof(errors));
        this.services = services ?? throw new ArgumentNullException(nameof(services));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        roles = uow.Set<Role>();
        actionForRole = uow.Set<ActionForRole>();

    }

    public async Task<int> AddActionForRole(CustomRole customRole)
    {
        var list = await actionForRole.Where(x => x.RoleId == customRole.Id).ToListAsync();
        foreach (var item in list)
            actionForRole.Remove(item);

        if (!string.IsNullOrEmpty(customRole.ActionList))
            foreach (var item in customRole.ActionList.Split(','))
            {
                if (item != null)
                {
                    var action = new ActionForRole
                    {
                        RoleId = customRole.Id,
                        AmActionId = int.Parse(item)
                    };
                    actionForRole.Add(action);
                }
            }
        return await uow.SaveChangesAsync();
    }

    public async Task<Role> CreateAsyncAndReturnRole(Role role)
    {
        await CreateAsync(role);
        return await roleManager.FindByNameAsync(role.Name);
    }
    public Task<IdentityResult> CreateAsync(Role role)
    {
        return roleManager.CreateAsync(role);
    }
    public Task<Role> FindByNameAsync(string roleName)
    {
        return roleManager.FindByNameAsync(roleName);
    }

    public IQueryable<Role> GetRoles()
    {
        return roles.AsQueryable();
    }

    public async Task<List<int>> GetUserRolse(int userid)
    {
        return await context.UserRoles.Where(x => x.UserId == userid).Select(x => x.RoleId).ToListAsync();
    }

    public Task<bool> RoleExistsAsync(string rolename)
    {
        return roles.AnyAsync(c => c.Name == rolename);
    }

    public List<int> GetUserRoles(int userid)
    {
        return context.UserRoles.Where(x => x.UserId == userid).Select(x => x.RoleId).ToList();
    }

    public List<string> GetUserRoleNames(string username)
    {
        var roles = from ur in context.UserRoles
                    join r in context.Roles on ur.RoleId equals r.Id
                    join u in context.Users on ur.UserId equals u.Id
                    where u.UserName == username
                    select r.Name;
        return roles.ToList();
    }

    public IQueryable<string> GetUsersInRole(string rolename)
    {
        var usernames = from ur in context.UserRoles
                        join r in context.Roles on ur.RoleId equals r.Id
                        join u in context.Users on ur.UserId equals u.Id
                        where r.Name == rolename
                        select u.UserName;
        return usernames;
    }

    public IQueryable<ApplicationUser> GetApplicationUsersInRole(string rolename)
    {
        var users = from ur in context.UserRoles
                        join r in context.Roles on ur.RoleId equals r.Id
                        join u in context.Users on ur.UserId equals u.Id
                        where r.Name == rolename
                        select u;
        return users;
    }
}
