using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Models;

namespace HelloChatApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private ApplicationUser? _currentUser;

        public UserService(IUserRepository userRepository)
        {
            _userRepository=userRepository;
        }

        public async Task<string?> GetLoggedUserName(HttpContext? context)
        {
            throw new NotImplementedException();
        }
    }
}
