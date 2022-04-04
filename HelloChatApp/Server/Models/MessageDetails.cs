namespace HelloChatApp.Server.Models
{
    public class MessageDetails
    {
        public string Room { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string UserHubId { get; set; }

        public MessageDetails(string room, string message, string userName, string userHubId)
        {
            Room = room;
            Message = message;
            UserName = userName;
            UserHubId = userHubId;
        }
    }
}
