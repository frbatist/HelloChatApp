using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Models;

namespace HelloChatApp.Server.Services
{
    public class MessageSenderFactory : IMessageSenderFactory
    {
        private readonly ICommandMessageSender _commandMessageSender;
        private readonly IChatMessageSender _chatMessageSender;

        public MessageSenderFactory(ICommandMessageSender commandMessageSender, IChatMessageSender chatMessageSender)
        {
            _commandMessageSender = commandMessageSender;
            _chatMessageSender = chatMessageSender;
        }

        public IMessageSender Get(MessageDetails messageDetails)
        {
            if (messageDetails.IsCommand)
                return _commandMessageSender;
            return _chatMessageSender;
        }
    }
}
