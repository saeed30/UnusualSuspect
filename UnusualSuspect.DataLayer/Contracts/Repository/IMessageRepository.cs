using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Contracts.Repository
{
  public interface IMessageRepository : IAsyncRepository<Message>
  {
    DateTime? GetUserLastMessageTime(int userId);
  }
}
