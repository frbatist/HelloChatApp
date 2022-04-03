using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HelloChatApp.Server.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager=userManager;
        }

        public Task<ApplicationUser> FindByClaimsPrincipal(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? String.Empty;
            return _userManager.FindByIdAsync(userId);
        }
    }
}
