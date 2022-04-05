using HelloChatApp.Server.Models;

namespace HelloChatApp.Server.Domain.Abstractions
{
    public interface IMessageSender
    {
        Task SendMessage(MessageDetails messageDetails);
    }
}
