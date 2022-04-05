using HelloChatApp.Server.Consumers;
using HelloChatApp.Server.Data.Repositories;
using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Services;
using HelloChatApp.Shared.Abstractions;
using HelloChatApp.Shared.Messages;

namespace HelloChatApp.Server.Extensions
{
    public static class ChatAppConfiguration
    {
        public static IServiceCollection ConfigureChatAppDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageProcessor, MessageProcessor>();
            services.AddScoped<IMessageSenderFactory, MessageSenderFactory>();
            services.AddScoped<ICommandMessageSender, CommandMessageSender>();
            services.AddScoped<IChatMessageSender, ChatMessageSender>();

            //Consumers
            services.AddScoped<IConsumer<LastStockPrice>, LastStockPriceConsumer>();

            return services;
        }
    }
}
