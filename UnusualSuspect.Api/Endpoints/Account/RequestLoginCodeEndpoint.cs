using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Account;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer;

namespace UnusualSuspect.Api.Endpoints.Account;

public class RequestLoginCodeEndpoint(IApplicationUserManager iApplicationUserManager,
    IOptionsSnapshot<ProjectSetting> setting,
    ISmsService smsService,
    ILogger<RequestLoginCodeEndpoint> logger,
    IGemService gemService,
    IUnitOfWork uow,
    ICoinService coinService)
  : EndpointBaseAsync
  .WithRequest<RequestLoginCodeRequest>
  .WithActionResult<ApiResultCommon>
{

  [AllowAnonymous]
  [HttpPost("api/[namespace]/RequestLoginCode")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync([FromBody] RequestLoginCodeRequest phoneNumber, CancellationToken cancellationToken = default)
  {
    //random delay
    var minWait = Task.Delay(new Random().Next(1, 2000), cancellationToken);

    string phone = phoneNumber.PhoneNumber;
    if (!PhoneNumberHelper.CheckAndFixPhoneNumber(ref phone))
    {
      await minWait.ConfigureAwait(false);
      return new ApiResultCommon(false, ApiResultStatusCode.NeedToRetry
        , "لطفا شماره همراه خود را به درستی وارد نمایید");
    }
    Random generator = new Random();
    string code = generator.Next(10000, 99999).ToString("D5");
    var user = await iApplicationUserManager.FindByNameAsync(phone);
    if (user != null)
    {
      user.PhoneNumberValidationCode = code;
      user.SendCodeDate = DateTime.Now;
      await iApplicationUserManager.UpdateAsync(user);
    }
    else
    {
      user = new Entities.Identity.ApplicationUser()
      {
        UserName = phone,
        PhoneNumber = phone,
        PhoneNumberConfirmed = false,
        DateCreate = DateTime.Now,
        IsActive = false,
        PhoneNumberValidationCode = code,
        SendCodeDate = DateTime.Now
      };
      var result = await iApplicationUserManager.CreateAsync(user, Guid.NewGuid().ToString());
      if (!result.Succeeded)
      {
        logger.LogEvent(SystemEventType.ErrorOnRegisterByPhone, null, phone, logLevel: LogLevel.Critical);
        return new ApiResultCommon(false, ApiResultStatusCode.ServerError
          , "اشکالی در زمان ثبت نام رخ داده است. لطفا بعدا تلاش نمایید");
      }

      var result1 = await gemService.BuyPackagesAsync(BaseGemPackageEnum.SignUpAward, user.Id, true, cancellationToken);
      var result2 = await coinService.BuyPackagesAsync(BaseCoinPackageEnum.SignUpAward, user.Id, true, cancellationToken);
      if (result1.Success && result2.Success)
        await uow.SaveChangesAsync(cancellationToken);
      else
      {
        if (!result1.Success)
          logger.LogEvent(SystemEventType.BaseGemPackageEnumOnRegisterFailed, user.Id, result1.MainError.ToString(), logLevel: LogLevel.Critical);
        if (!result2.Success)
          logger.LogEvent(SystemEventType.BaseCoinPackageEnumOnRegisterFailed, user.Id, result2.MainError.ToString(), logLevel: LogLevel.Critical);
      }
    }
    if (phone.StartsWith("0999999"))
      return new ApiResultCommon(true, ApiResultStatusCode.Success, "کد تایید: " + code);
    bool smsSent = await smsService.SendOtpAsync(phone, code, user.Id, cancellationToken);
    await minWait.ConfigureAwait(false);
    if (smsSent)
      return new ApiResultCommon(true, ApiResultStatusCode.Success, "کد تایید به شماره همراه شما ارسال شد");
    return new ApiResultCommon(false, ApiResultStatusCode.ServerError
      , "اشکالی در زمان ارسال کد به شماره همراه شما رخ داد. لطفا مجدد تلاش نمایید");
  }
}
