using HelloChatApp.Server.Models;

namespace HelloChatApp.Server.Domain.Abstractions
{
    public interface IMessageSenderFactory
    {
        IMessageSender Get(MessageDetails messageDetails);
    }
}
