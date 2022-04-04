namespace HelloChatApp.Shared.Messages
{
    public class StockQueryCommand
    {
        public string? UserName { get; set; }
        public string? UserHubId { get; set; }
        public string? StockCode { get; set; }
    }
}
