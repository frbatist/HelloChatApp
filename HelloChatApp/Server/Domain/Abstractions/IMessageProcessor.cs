using HelloChatApp.Server.Models;
using HelloChatApp.Shared;

namespace HelloChatApp.Server.Domain.Abstractions
{
    public interface IMessageProcessor
    {
        Task ProcessMessage(MessageDetails messageDetails);
    }
}
