using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IPaymentUserRepository : IAsyncRepository<PaymentUser>
{
  IQueryable<PaymentUser> Search(PaymentUserSearchFilterDto filterDto);
}