using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Models;

namespace HelloChatApp.Server.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IMessageSenderFactory _messageSenderFactory;

        public MessageProcessor(IMessageSenderFactory messageSenderFactory)
        {
            _messageSenderFactory = messageSenderFactory;
        }

        public Task ProcessMessage(MessageDetails messageDetails)
        {
            return _messageSenderFactory.Get(messageDetails).SendMessage(messageDetails);
        }        
    }
}
