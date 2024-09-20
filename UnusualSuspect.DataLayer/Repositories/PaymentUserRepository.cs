using Microsoft.Extensions.Logging;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.DataLayer.Repositories;

public sealed class PaymentUserRepository(IUnitOfWork uow, ILogger<PaymentUserRepository> logger)
  : EfRepository<PaymentUser>(uow, logger), IPaymentUserRepository
{
  public IQueryable<PaymentUser> Search(PaymentUserSearchFilterDto filterDto)
  {
    IQueryable<PaymentUser> result = BaseEntity;
    if (filterDto.UserId.HasValue)
      result = result.Where(x => x.UserId == filterDto.UserId.Value);
    if (filterDto.MinAmount.HasValue)
      result = result.Where(x => x.Amount >= filterDto.MinAmount.Value);

    switch (filterDto.IsValid)
    {
      case NullableBoolValuesEnum.None:
        break;
      case NullableBoolValuesEnum.True:
        result = result.Where(x => x.IsValid == true);
        break;
      case NullableBoolValuesEnum.False:
        result = result.Where(x => x.IsValid == false);
        break;
      case NullableBoolValuesEnum.Null:
        result = result.Where(x => !x.IsValid.HasValue);
        break;
      case NullableBoolValuesEnum.NotNull:
        result = result.Where(x => x.IsValid.HasValue);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }

    return result;
  }
}