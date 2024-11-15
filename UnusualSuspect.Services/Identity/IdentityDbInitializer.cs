using Aspose.Cells;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Identity;
public class IdentityDbInitializer(IServiceScopeFactory scopeFactory,
    IApplicationUserManager applicationUserManager,
    IApplicationRoleService applicationRoleManager,
    IOptionsSnapshot<ProjectSetting> adminUserSeedOptions,
    ILogger<IdentityDbInitializer> logger,
    IUnitOfWork uow,
    IDapperRepository dapperRepository
    )
  : IIdentityDbInitializer
{
  public void Initialize()
  {
    using (var serviceScope = scopeFactory.CreateScope())
    {
      using (var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
      {
        context.Database.Migrate();
      }
    }
  }
  public async void SeedData()
  {
    using var serviceScope = scopeFactory.CreateScope();
    var identityDbSeedData = serviceScope.ServiceProvider.GetRequiredService<IIdentityDbInitializer>();
    var result = await identityDbSeedData.SeedDatabaseWithAdminUserAsync();
    if (result == IdentityResult.Failed())
    {
      throw new InvalidOperationException(result.DumpErrors());
    }
  }
  public async Task<IdentityResult> SeedDatabaseWithAdminUserAsync()
  {
    await InsertOrUpdateBaseData();
    await InserBotUsers(50);
    var adminUserSeed = adminUserSeedOptions.Value.AdminUser;
    if (adminUserSeed == null) return IdentityResult.Success;
    var name = adminUserSeed.Username;
    var password = adminUserSeed.Password;
    var email = adminUserSeed.Email;
    var roleName = adminUserSeed.RoleName;

    var thisMethodName = nameof(SeedDatabaseWithAdminUserAsync);
    ApplicationUser? adminUser;
    try
    {
      adminUser = await applicationUserManager.FindByNameAsync(name);
    }
    catch (Exception e)
    {
      logger.LogEvent(SystemEventType.SeedUserTableNotCreated, null, logLevel: LogLevel.Error, exception: e);
      return IdentityResult.Success;
    }
    if (adminUser != null)
    {
      //logger.LogInformation($"{thisMethodName}: adminUser already exists.");
      //return IdentityResult.Success;
    }

    var adminRole = await applicationRoleManager.FindByNameAsync(roleName);
    if (adminRole == null)
    {
      adminRole = new Role(roleName);
      var adminRoleResult = await applicationRoleManager.CreateAsync(adminRole);
      if (adminRoleResult == IdentityResult.Failed())
      {
        logger.LogEvent(SystemEventType.SeedAdminRoleCreateFailed, null,
          $"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}", logLevel: LogLevel.Critical);
        //return IdentityResult.Failed();
      }
    }

    var adminRole2 = await applicationRoleManager.FindByNameAsync("CustomerRole");
    if (adminRole2 == null)
    {
      adminRole2 = new Role("CustomerRole");
      var customerRoleResult = await applicationRoleManager.CreateAsync(adminRole2);
      if (customerRoleResult == IdentityResult.Failed())
      {
        logger.LogEvent(SystemEventType.SeedCustomerRoleCreateFailed, null,
          $"{thisMethodName}: CustomerRole CreateAsync failed. {customerRoleResult.DumpErrors()}", logLevel: LogLevel.Critical);
        //return IdentityResult.Failed();
      }
    }
    //else
    //	logger.LogInformation($"{thisMethodName}: CustomerRole already exists.");

    var adminRole3 = await applicationRoleManager.FindByNameAsync("AdminPanelUserRole");
    if (adminRole3 == null)
    {
      adminRole3 = new Role("AdminPanelUserRole");
      var adminRoleResult = await applicationRoleManager.CreateAsync(adminRole3);
      if (adminRoleResult == IdentityResult.Failed())
      {
        logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
        //return IdentityResult.Failed();
      }
    }
    //else
    //	logger.LogInformation($"{thisMethodName}: adminRole already exists.");

    adminUser = new ApplicationUser
    {
      UserName = name,
      Email = email,
      EmailConfirmed = true,
      LockoutEnabled = true,
      IsActive = true,
      SecurityStamp = Guid.NewGuid().ToString()
    };
    var adminUserResult = await applicationUserManager.CreateAsync(adminUser, password);
    if (adminUserResult == IdentityResult.Failed())
    {
      logger.LogError($"{thisMethodName}: adminUser CreateAsync failed. {adminUserResult.DumpErrors()}");
      return IdentityResult.Failed();
    }

    var setLockoutResult = await applicationUserManager.SetLockoutEnabledAsync(adminUser, enabled: false);
    if (setLockoutResult == IdentityResult.Failed())
    {
      logger.LogError($"{thisMethodName}: adminUser SetLockoutEnabledAsync failed. {setLockoutResult.DumpErrors()}");
      return IdentityResult.Failed();
    }

    var addToRoleResult = await applicationUserManager.AddToRoleAsync(adminUser, adminRole.Name);
    if (addToRoleResult == IdentityResult.Failed())
    {
      logger.LogError($"{thisMethodName}: adminUser AddToRoleAsync failed. {addToRoleResult.DumpErrors()}");
      return IdentityResult.Failed();
    }

    return IdentityResult.Success;
  }

  private async Task InserBotUsers(int numberOfBots)
  {
    try
    {
      ApplicationUser? adminUserResult = await applicationUserManager.FindByNameAsync("bot" + (numberOfBots - 1));
      if (adminUserResult != null)
        return;
      for (int i = 0; i < numberOfBots; i++)
      {
        adminUserResult = await applicationUserManager.FindByNameAsync("bot" + i);
        if (adminUserResult != null)
          continue;
        ApplicationUser User = new ApplicationUser
        {
          UserName = "bot" + i,
          Email = "bot" + i + "@site.com",
          EmailConfirmed = true,
          LockoutEnabled = true,
          IsActive = true,
          SecurityStamp = Guid.NewGuid().ToString(),
          IsBot = true,
          NickName = "user" + i
        };
        await applicationUserManager.CreateAsync(User, Guid.NewGuid().ToString());
      }
    }
    catch { }//error before new column is added
  }

  private async Task InsertOrUpdateBaseData()
  {
    await HasDataForEnumEntity<ReadyToGameStatus, ReadyToGameStatusEnum>("ReadyToGameStatus");
    await HasDataForEnumEntity<PreGameGroupStatus, PreGameGroupStatusEnum>("PreGameGroupStatus");
    await HasDataForEnumEntity<SmsSendingStatus, SmsSendingStatusEnum>("SmsSendingStatus");
    await HasDataForEnumEntity<RoleCard, RoleCardEnum>("RoleCard");
    await HasDataForEnumEntity<GameStatus, GameStatusEnum>("GameStatus");
    await HasDataForEnumEntity<ScoreType, ScoreTypeEnum>("ScoreType");
    await HasDataForEnumEntity<GemPackage, BaseGemPackageEnum>("GemPackage");
    await HasDataForEnumEntity<CoinPackage, BaseCoinPackageEnum>("GemPackage");
    await HasDataForEnumEntity<PriceType, PriceTypeEnum>("PriceType");
    await HasDataForEnumEntity<Store, StoreEnum>("Store");
    await HasDataForEnumEntity<RepetitionType, RepetitionTypeEnum>("RepetitionType");
  }

  private async Task HasDataForEnumEntity<TEntity, TEnum>(string tableName)
    where TEntity : BaseEnumEntity, new() where TEnum : Enum
  {
    try
    {
      int? result = await dapperRepository.QuerySingleAsync<int?>($@"SELECT 1 FROM sys.tables AS T 
INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id
WHERE T.Name = '{tableName}'");
      if (!result.HasValue)
        return;
      DbSet<TEntity> baseEntity = uow.Set<TEntity>();
      List<TEntity> entities = await baseEntity.ToListAsync();
      bool newChange = false;
      foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
      {
        short id = Convert.ToInt16(enumValue);
        TEntity? model = entities.FirstOrDefault(x => x.Id == id);
        if (model == null)
        {
          TEntity newModel = new TEntity()
          {
            Id = id,
            Title = enumValue.ToDisplay(),
            Name = enumValue.ToString()
          };
          SpecificChangesBasedOnEntity(ref newModel);
          baseEntity.Add(newModel);
          newChange = true;
        }
        else
        {
          if (model.Name != enumValue.ToString())
          {
            model.Name = enumValue.ToString();
            newChange = true;
          }
          //Title may change by admin
        }
      }
      if (newChange)
        await uow.SaveChangesAsync();
    }
    catch (Exception e)
    {
      logger.LogError(e, "Error while init data for table {tableName}", tableName);
    }
  }

  private void SpecificChangesBasedOnEntity<TEntity>(ref TEntity model) where TEntity : BaseEnumEntity, new()
  {
    if (model is GemPackage entityGem)
    {
      entityGem.ImageUrl = "";
      entityGem.IsActive = true;
      entityGem.IsPublic = false;
    }
    if (model is CoinPackage entityCoin)
    {
      entityCoin.ImageUrl = "";
      entityCoin.IsActive = true;
      entityCoin.IsPublic = false;
    }
  }
}
