using HelloChatApp.Server.Models;
using System.Security.Claims;

namespace HelloChatApp.Server.Domain.Abstractions
{
    /// <summary>
    /// Abstraction of methods from UserManager
    /// </summary>
    public interface IUserRepository
    {
        Task<ApplicationUser> FindByClaimsPrincipal(ClaimsPrincipal principal);
    }
}
