using ElmahCore;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
  private async Task RecalculateGemByUserId(ApplicationUser user)
  {
    int result = await gemPackageUserRepository.GetSumAsync(user.Id) -
                 await gemUsedUserRepository.GetSumAsync(user.Id);
    if (result < 0)
      logger.LogCritical("User CalculatedGems is below zero. userId: {userId}, calculatedGems: {CalculatedGems}", user.Id, user.CalculatedGems);
    if (user.CalculatedGems != result)
    {
      logger.LogWarning("User CalculatedGems({CalculatedGems}) is not equal to user newCalculatedGems({newCalculatedGems}). userId: {userId}", result, user.CalculatedGems, user.Id);
      user.CalculatedGems = result;
    }
  }
  private async Task RecalculateCoinByUserId(ApplicationUser user)
  {
    int result = await coinPackageUserRepository.GetSumAsync(user.Id) -
                 await coinUsedUserRepository.GetSumAsync(user.Id);
    if (result < 0)
      logger.LogCritical("User CalculatedCoins is below zero. userId: {userId}, calculatedCoins: {CalculatedCoins}", user.Id, user.CalculatedCoins);
    if (user.CalculatedCoins != result)
    {
      logger.LogWarning("User CalculatedCoins({CalculatedCoins}) is not equal to user newCalculatedCoins({CalculatedCoins}). userId: {userId}", result, user.CalculatedCoins, user.Id);
      user.CalculatedCoins = result;
    }
  }
}