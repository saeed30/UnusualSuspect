using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Services.Services;

public sealed class MessageService(IMessageRepository messageRepository,
  IMessageReceiverRepository messageReceiverRepository,
  IApplicationUserManager userManager) : IMessageService
{
  public UnusualSuspectServiceResult<bool> SendMessageToAdmin(string message, int senderUserId)
  {
    ApplicationUser? admin = userManager.FindByName("admin");
    if(admin == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.AdminUserNotFound));
    return SendMessage(message, senderUserId, admin.Id);
  }

  public UnusualSuspectServiceResult<bool> SendMessage(string message, int senderUserId, int receiverUserId)
  {
    return SendMessage(message, senderUserId, new List<int> { receiverUserId });
  }

  public UnusualSuspectServiceResult<bool> SendMessage(string message, int senderUserId, List<int> receiverUserId)
  {
    if (message.Length > 4000)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.MessageContentLengthTooLong));
    if (!receiverUserId.Any())
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.CanNotSendMessageWithoutReceiver));
    DateTime? lastMessageDate = messageRepository.GetUserLastMessageTime(senderUserId);
    if(lastMessageDate.HasValue && lastMessageDate.Value.AddSeconds(3) > DateTime.Now)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.MessageSendIntervalIsLowerThanAcceptedLimit));
    Message msg = messageRepository.Add(new Message()
    {
      IsHidden = false,
      Deleted = false,
      MessageContent = message,
      SendTime = DateTime.Now,
      SenderUserId = senderUserId
    });
    foreach (int userId in receiverUserId)
    {
      messageReceiverRepository.Add(new MessageReceiver()
      {
        Deleted = false,
        IsViewed = false,
        Message = msg,
        ReceiverUserId = userId,
        ViewTime = null
      });
    }
    return new UnusualSuspectServiceResult<bool>(true);
  }
}