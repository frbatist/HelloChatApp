namespace HelloChatApp.Shared.Messages
{
    public class LastStockPrice
    {
        public string? Room { get; set; }
        public string? UserName { get; set; }
        public string? UserHubId { get; set; }
        public string? StockCode { get; set; }
        public double StockPrice { get; set; }
    }
}
