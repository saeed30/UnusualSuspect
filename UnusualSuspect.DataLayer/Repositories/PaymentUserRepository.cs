using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories
{
  public sealed class PaymentUserRepository(IUnitOfWork uow, ILogger<PaymentUserRepository> logger)
    : EfRepository<PaymentUser>(uow, logger), IPaymentUserRepository
  {
  }
}
