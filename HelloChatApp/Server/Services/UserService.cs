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
            if (context == null)
                return null;
            if (_currentUser == null)
            {
                var principal = context.User;
                _currentUser = await _userRepository.FindByClaimsPrincipal(principal);
            }
            if (_currentUser == null)
                return null;
            return _currentUser.FirstName;
        }
    }
}
