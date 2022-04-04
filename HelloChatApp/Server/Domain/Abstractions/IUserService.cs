namespace HelloChatApp.Server.Domain.Abstractions
{
    public interface IUserService
    {
        Task<string?> GetLoggedUserName(HttpContext? context);
    }
}
