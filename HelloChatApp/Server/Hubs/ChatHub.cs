using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HelloChatApp.Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;

        public ChatHub(IUserService userService)
        {
            _userService = userService;
        }

        public async Task SendMessage(MessageModel model)
        {
            var aaa = Context.ConnectionId;
            var user = await _userService.GetLoggedUserName(Context.GetHttpContext());
            await Clients.All.SendAsync(model.Room, user, model.Message);
        }
    }
}
