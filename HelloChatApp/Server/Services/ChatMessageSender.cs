using HelloChatApp.Server.Hubs;
using HelloChatApp.Server.Models;
using Microsoft.AspNetCore.SignalR;

namespace HelloChatApp.Server.Services
{
    public class ChatMessageSender : IChatMessageSender
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatMessageSender(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendMessage(MessageDetails messageDetails)
        {
            return _hubContext.Clients.All.SendAsync(messageDetails.Room, messageDetails.UserName, messageDetails.Message);
        }
    }
}
