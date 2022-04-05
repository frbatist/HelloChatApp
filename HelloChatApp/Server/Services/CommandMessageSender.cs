using HelloChatApp.Server.Hubs;
using HelloChatApp.Server.Models;
using HelloChatApp.Shared.Abstractions;
using HelloChatApp.Shared.Messages;
using Microsoft.AspNetCore.SignalR;

namespace HelloChatApp.Server.Services
{
    public class CommandMessageSender : ICommandMessageSender
    {
        private const string StockQueryCommandExchange = "stock-query-command";
        private const char CommandValueSeparator = '=';
        private const string DefaultCommand = "stock";
        private const string ChatBotUserName = "Stock Bot";
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<CommandMessageSender> _logger;
        private readonly IPublisher _publisher;

        public CommandMessageSender(IHubContext<ChatHub> hubContext, ILogger<CommandMessageSender> logger, IPublisher publisher)
        {
            _hubContext = hubContext;
            _logger = logger;
            _publisher = publisher;
        }

        public async Task SendMessage(MessageDetails messageDetails)
        {
            var args = messageDetails.Message.Substring(1, messageDetails.Message.Length - 1).Split(CommandValueSeparator);
            if (args[0] == DefaultCommand)
            {
                _logger.LogDebug($"Message is a command - {DefaultCommand}!");

                if (args.Length < 2 || string.IsNullOrEmpty(args[1]))
                {
                    string errorMessage = $"Command invalid: {args[0]}. The value must be informed!";
                    await SendErrorMessageToUser(messageDetails, errorMessage);
                }
                else
                {
                    SendCommandToBroker(messageDetails, args);
                }
            }
            else
            {
                string errorMessage = $"Message is not a valid command - {args[0]}!";
                await SendErrorMessageToUser(messageDetails, errorMessage);
            }
        }

        private void SendCommandToBroker(MessageDetails messageDetails, string[] args)
        {
            var command = new StockQueryCommand
            {
                Room = messageDetails.Room,
                StockCode = args[1],
                UserHubId = messageDetails.UserHubId,
                UserName = messageDetails.UserName
            };
            _publisher.Publish(StockQueryCommandExchange, command);
        }

        private Task SendErrorMessageToUser(MessageDetails messageDetails, string errorMessage)
        {
            _logger.LogDebug(errorMessage);
            return _hubContext.Clients.Client(messageDetails.UserHubId).SendAsync(messageDetails.Room, ChatBotUserName, errorMessage);
        }
    }
}
