using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Identity;
public class IdentityDbInitializer : IIdentityDbInitializer
{
	private readonly IOptionsSnapshot<ProjectSetting> _adminUserSeedOptions;
	private readonly ILogger<IdentityDbInitializer> _logger;
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly IApplicationUserManager _applicationUserManager;
	private readonly IApplicationRoleService _applicationRoleManager;

	public IdentityDbInitializer(
			IServiceScopeFactory scopeFactory,
			IApplicationUserManager applicationUserManager,
			IApplicationRoleService applicationRoleManager,
			IOptionsSnapshot<ProjectSetting> adminUserSeedOptions,
			ILogger<IdentityDbInitializer> logger
			)
	{
		_scopeFactory = scopeFactory;
		_applicationUserManager = applicationUserManager;
		_applicationRoleManager = applicationRoleManager;
		_adminUserSeedOptions = adminUserSeedOptions;
		_logger = logger;
	}
	public void Initialize()
	{
		using (var serviceScope = _scopeFactory.CreateScope())
		{
			using (var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
			{
				context.Database.Migrate();
			}
		}
	}
	public async void SeedData()
	{
		using var serviceScope = _scopeFactory.CreateScope();
		var identityDbSeedData = serviceScope.ServiceProvider.GetRequiredService<IIdentityDbInitializer>();
		var result = await identityDbSeedData.SeedDatabaseWithAdminUserAsync();
		if (result == IdentityResult.Failed())
		{
			throw new InvalidOperationException(result.DumpErrors());
		}
	}
	public async Task<IdentityResult> SeedDatabaseWithAdminUserAsync()
	{
		var adminUserSeed = _adminUserSeedOptions.Value.AdminUser;
		if (adminUserSeed == null) return IdentityResult.Success;
		var name = adminUserSeed.Username;
		var password = adminUserSeed.Password;
		var email = adminUserSeed.Email;
		var roleName = adminUserSeed.RoleName;

		var thisMethodName = nameof(SeedDatabaseWithAdminUserAsync);
		ApplicationUser? adminUser;
		try
		{
			adminUser = await _applicationUserManager.FindByNameAsync(name);
		}
		catch (Exception e)
		{
			_logger.LogError("users table not created");
			return IdentityResult.Success;
		}
		if (adminUser != null)
		{
			_logger.LogInformation($"{thisMethodName}: adminUser already exists.");
			//return IdentityResult.Success;
		}

		var adminRole = await _applicationRoleManager.FindByNameAsync(roleName);
		if (adminRole == null)
		{
			adminRole = new Role(roleName);
			var adminRoleResult = await _applicationRoleManager.CreateAsync(adminRole);
			if (adminRoleResult == IdentityResult.Failed())
			{
				_logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
				//return IdentityResult.Failed();
			}
		}
		else
			_logger.LogInformation($"{thisMethodName}: adminRole already exists.");

		var adminRole2 = await _applicationRoleManager.FindByNameAsync("CustomerRole");
		if (adminRole2 == null)
		{
			adminRole2 = new Role("CustomerRole");
			var adminRoleResult = await _applicationRoleManager.CreateAsync(adminRole2);
			if (adminRoleResult == IdentityResult.Failed())
			{
				_logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
				//return IdentityResult.Failed();
			}
		}
		else
			_logger.LogInformation($"{thisMethodName}: adminRole already exists.");

		var adminRole3 = await _applicationRoleManager.FindByNameAsync("AdminPanelUserRole");
		if (adminRole3 == null)
		{
			adminRole3 = new Role("AdminPanelUserRole");
			var adminRoleResult = await _applicationRoleManager.CreateAsync(adminRole3);
			if (adminRoleResult == IdentityResult.Failed())
			{
				_logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
				//return IdentityResult.Failed();
			}
		}
		else
			_logger.LogInformation($"{thisMethodName}: adminRole already exists.");

		adminUser = new ApplicationUser
		{
			UserName = name,
			Email = email,
			EmailConfirmed = true,
			LockoutEnabled = true,
			IsActive = true
		};
		var adminUserResult = await _applicationUserManager.CreateAsync(adminUser, password);
		if (adminUserResult == IdentityResult.Failed())
		{
			_logger.LogError($"{thisMethodName}: adminUser CreateAsync failed. {adminUserResult.DumpErrors()}");
			return IdentityResult.Failed();
		}

		var setLockoutResult = await _applicationUserManager.SetLockoutEnabledAsync(adminUser, enabled: false);
		if (setLockoutResult == IdentityResult.Failed())
		{
			_logger.LogError($"{thisMethodName}: adminUser SetLockoutEnabledAsync failed. {setLockoutResult.DumpErrors()}");
			return IdentityResult.Failed();
		}

		var addToRoleResult = await _applicationUserManager.AddToRoleAsync(adminUser, adminRole.Name);
		if (addToRoleResult == IdentityResult.Failed())
		{
			_logger.LogError($"{thisMethodName}: adminUser AddToRoleAsync failed. {addToRoleResult.DumpErrors()}");
			return IdentityResult.Failed();
		}

		return IdentityResult.Success;
	}
}
