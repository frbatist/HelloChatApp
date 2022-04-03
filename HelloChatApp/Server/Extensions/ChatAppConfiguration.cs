using HelloChatApp.Server.Data.Repositories;
using HelloChatApp.Server.Domain.Abstractions;

namespace HelloChatApp.Server.Extensions
{
    public static class ChatAppConfiguration
    {
        public static IServiceCollection ConfigureChatAppDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();            
            return services;
        }
    }
}
