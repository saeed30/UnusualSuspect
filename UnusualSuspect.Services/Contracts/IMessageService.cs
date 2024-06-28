
namespace UnusualSuspect.Services.Contracts;

public interface IMessageService
{
  UnusualSuspectServiceResult<bool> SendMessageToAdmin(string message, int senderUserId);
  UnusualSuspectServiceResult<bool> SendMessage(string message,
    int senderUserId, int receiverUserId);
  UnusualSuspectServiceResult<bool> SendMessage(string message,
    int senderUserId, List<int> receiverUserId);
}