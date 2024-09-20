using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Repositories
{
  public class PaymentCafeBazaarRepository(IUnitOfWork uow, ILogger<PaymentCafeBazaarRepository> logger)
    : EfRepository<PaymentCafeBazaar>(uow, logger), IPaymentCafeBazaarRepository
  {
  }
}
