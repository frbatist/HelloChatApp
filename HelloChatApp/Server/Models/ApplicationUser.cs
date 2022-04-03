using Microsoft.AspNetCore.Identity;

namespace HelloChatApp.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public ApplicationUser(string firstName)
        {
            FirstName=firstName;
        }
    }
}