using Microsoft.EntityFrameworkCore;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.Services.Services;

public class PaymentUserService(IPaymentUserRepository paymentUserRepository, IApiCallService apiCallService, IUnitOfWork uow) : IPaymentUserService
{
  public async Task CheckAllUncheckedPayments(CancellationToken cancellationToken = default)
  {
    var result = paymentUserRepository.Search(new PaymentUserSearchFilterDto(null, "", null, 1, NullableBoolValuesEnum.Null));
    //Cafebazaar
    foreach (PaymentUser paymentUser in await result.Where(x => x.StoreId == (short)StoreEnum.Cafebazaar).ToListAsync(cancellationToken))
    {
      UnusualSuspectServiceResult<(bool, string?)> apiResult = await apiCallService.CheckPaymentInCafebazaar(paymentUser, cancellationToken);
      if (!apiResult.Success)
        continue;
      paymentUser.IsValid = apiResult.Result.Item1;
      paymentUser.ValidationCheckDateTime = DateTime.Now;
      paymentUser.ValidationError = apiResult.Result.Item2;
      await uow.SaveChangesAsync(cancellationToken);
    }
    //Myket
    foreach (PaymentUser paymentUser in await result.Where(x => x.StoreId == (short)StoreEnum.Myket).ToListAsync(cancellationToken))
    {
      UnusualSuspectServiceResult<(bool, string?)> apiResult = await apiCallService.CheckPaymentInMyket(paymentUser, cancellationToken);
      if (!apiResult.Success)
        continue;
      paymentUser.IsValid = apiResult.Result.Item1;
      paymentUser.ValidationCheckDateTime = DateTime.Now;
      paymentUser.ValidationError = apiResult.Result.Item2;
      await uow.SaveChangesAsync(cancellationToken);
    }
  }
}