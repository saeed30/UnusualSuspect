using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Repositories
{
  public sealed class MessageReceiverRepository(IUnitOfWork uow, ILogger<MessageReceiverRepository> logger) :
    EfRepository<MessageReceiver>(uow, logger), IMessageReceiverRepository
  {
  }
}
