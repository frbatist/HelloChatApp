using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Models;
using HelloChatApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HelloChatApp.Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly IMessageProcessor _messageProcessor;

        public ChatHub(IUserService userService, IMessageProcessor messageProcessor)
        {
            _userService = userService;
            _messageProcessor=messageProcessor;
        }

        public async Task SendMessage(MessageModel model)
        {
            var user = await _userService.GetLoggedUserName(Context.GetHttpContext());
            await _messageProcessor.ProcessMessage(new MessageDetails(model.Room, model.Message, user, Context.ConnectionId));
        }
    }
}
