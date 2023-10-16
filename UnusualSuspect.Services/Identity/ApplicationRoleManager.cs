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
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUnitOfWork _uow;
    private readonly IdentityErrorDescriber _errors;
    private readonly ILookupNormalizer _keyNormalizer;
    private readonly ILogger<ApplicationRoleManager> _logger;
    private readonly IOptions<IdentityOptions> _optionsAccessor;
    private readonly IServiceProvider _services;
    private readonly DbSet<Role> _roles;
    private readonly DbSet<ActionForRole> _actionForRole;
    private readonly RoleManager<Role> _roleManager;
    private readonly ApplicationDbContext _context;

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
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(_optionsAccessor));
        _keyNormalizer = keyNormalizer ?? throw new ArgumentNullException(nameof(_keyNormalizer));
        _errors = errors ?? throw new ArgumentNullException(nameof(_errors));
        _services = services ?? throw new ArgumentNullException(nameof(_services));
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
        _context = context ?? throw new ArgumentNullException(nameof(_context));
        _roles = uow.Set<Role>();
        _actionForRole = uow.Set<ActionForRole>();

    }

    public async Task<int> AddActionForRole(CustomRole customRole)
    {
        var list = await _actionForRole.Where(x => x.RoleId == customRole.Id).ToListAsync();
        foreach (var item in list)
            _actionForRole.Remove(item);

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
                    _actionForRole.Add(action);
                }
            }
        return await _uow.SaveChangesAsync();
    }

    public async Task<Role> CreateAsyncAndReturnRole(Role role)
    {
        await CreateAsync(role);
        return await _roleManager.FindByNameAsync(role.Name);
    }
    public Task<IdentityResult> CreateAsync(Role role)
    {
        return _roleManager.CreateAsync(role);
    }
    public Task<Role> FindByNameAsync(string roleName)
    {
        return _roleManager.FindByNameAsync(roleName);
    }

    public IQueryable<Role> GetRoles()
    {
        return _roles.AsQueryable();
    }

    public async Task<List<int>> GetUserRolse(int userid)
    {
        return await _context.UserRoles.Where(x => x.UserId == userid).Select(x => x.RoleId).ToListAsync();
    }

    public Task<bool> RoleExistsAsync(string rolename)
    {
        return _roles.AnyAsync(c => c.Name == rolename);
    }

    public List<int> GetUserRoles(int userid)
    {
        return _context.UserRoles.Where(x => x.UserId == userid).Select(x => x.RoleId).ToList();
    }

    public List<string> GetUserRoleNames(string username)
    {
        var roles = from ur in _context.UserRoles
                    join r in _context.Roles on ur.RoleId equals r.Id
                    join u in _context.Users on ur.UserId equals u.Id
                    where u.UserName == username
                    select r.Name;
        return roles.ToList();
    }

    public IQueryable<string> GetUsersInRole(string rolename)
    {
        var usernames = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        join u in _context.Users on ur.UserId equals u.Id
                        where r.Name == rolename
                        select u.UserName;
        return usernames;
    }

    public IQueryable<ApplicationUser> GetApplicationUsersInRole(string rolename)
    {
        var users = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        join u in _context.Users on ur.UserId equals u.Id
                        where r.Name == rolename
                        select u;
        return users;
    }
}
