using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Repositories
{
  public sealed class MessageRepository(IUnitOfWork uow, ILogger<MessageRepository> logger) :
    EfRepository<Message>(uow, logger), IMessageRepository
  {
    public DateTime? GetUserLastMessageTime(int userId)
    {
      return BaseEntity.Where(x => x.SenderUserId == userId).OrderByDescending(x => x.SendTime).FirstOrDefault()?.SendTime;
    }
  }
}
