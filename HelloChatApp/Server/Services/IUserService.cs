namespace HelloChatApp.Server.Services
{
    public interface IUserService
    {
        Task<string?> GetLoggedUserName(HttpContext? context);
    }
}
