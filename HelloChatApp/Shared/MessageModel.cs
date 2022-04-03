namespace HelloChatApp.Shared
{
    public class MessageModel
    {
        public string Room { get; set; }
        public string Message { get; set; }

        public MessageModel(string room, string message)
        {
            Room = room;
            Message = message;
        }
    }
}
