using HelloChatApp.Server.Data.Repositories;
using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Services;

namespace HelloChatApp.Server.Extensions
{
    public static class ChatAppConfiguration
    {
        public static IServiceCollection ConfigureChatAppDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
