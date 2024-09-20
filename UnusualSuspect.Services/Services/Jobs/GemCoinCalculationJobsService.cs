using ElmahCore;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Services.Services.Jobs;

public sealed class GemCoinCalculationJobsService(IGemUsedUserRepository gemUsedUserRepository,
  IGemPackageUserRepository gemPackageUserRepository,
  ICoinUsedUserRepository coinUsedUserRepository,
  ICoinPackageUserRepository coinPackageUserRepository,
  IPaymentUserService paymentUserService,
  IUnitOfWork uow,
  ILogger<GemCoinCalculationJobsService> logger,
  IApplicationUserManager applicationUserManager,
  UserManager<ApplicationUser> userManager) : IGemCoinCalculationJobsService
{
  [DisableConcurrentExecution(1000)]
  public async Task RecalculateAllUsersGemAndCoin()
  {
    try
    {
      List<int> ids = await userManager.Users.Select(x => x.Id).ToListAsync();
      for (int i = 0; i < ids.Count; i++)
        await RecalculateGemAndCoinByUserId(ids[i]);
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
    }
  }
  [DisableConcurrentExecution(1000)]
  public async Task RecalculateGemAndCoinByUserId(int userId)
  {
    bool done = false;
    try
    {
      var user = await applicationUserManager.FindByIdAsync(userId.ToString());
      if (user != null)
      {
        await RecalculateCoinByUserId(user);
        await RecalculateGemByUserId(user);
        done = true;
      }
    }
    catch (Exception ex)
    {
      ElmahExtensions.RaiseError(ex);
    }
    if (done)
      await uow.SaveChangesAsync();
  }

  public async Task ValidatePayments(int userId)
  {
    await paymentUserService.CheckAllUncheckedPayments(userId);
  }

  private async Task RecalculateGemByUserId(ApplicationUser user)
  {
    int result = await gemPackageUserRepository.GetSumAsync(user.Id) -
                 await gemUsedUserRepository.GetSumAsync(user.Id);
    if (result < 0)
      logger.LogEvent(SystemEventType.UserCalculatedGemsBelowZero, user.Id,
        $"CalculatedGems({user.CalculatedGems}) - newCalculatedGems({result})", logLevel: LogLevel.Critical);
    if (user.CalculatedGems != result)
    {
      logger.LogEvent(SystemEventType.UserCalculatedGemsNotEqualToCurrentValue,
        user.Id, $"CalculatedGems({user.CalculatedGems}) - newCalculatedGems({result})", logLevel: LogLevel.Critical);
      user.CalculatedGems = result;
    }
  }
  private async Task RecalculateCoinByUserId(ApplicationUser user)
  {
    int result = await coinPackageUserRepository.GetSumAsync(user.Id) -
                 await coinUsedUserRepository.GetSumAsync(user.Id);
    if (result < 0)
      logger.LogEvent(SystemEventType.UserCalculatedCoinsBelowZero, user.Id,
        $"CalculatedCoins({user.CalculatedCoins}) - newCalculatedCoins({result})", logLevel: LogLevel.Critical);
    if (user.CalculatedCoins != result)
    {
      logger.LogEvent(SystemEventType.UserCalculatedCoinsNotEqualToCurrentValue, user.Id,
        $"CalculatedCoins({user.CalculatedCoins}) - newCalculatedCoins({result})", logLevel: LogLevel.Critical);
      user.CalculatedCoins = result;
    }
  }
}