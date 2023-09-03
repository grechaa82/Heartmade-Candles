namespace HeartmadeCandles.API.Contracts.Requests
{
    public class OrderRequest
    {
        public string ConfiguredCandlesString { get; set; }
        public UserRequest User { get; set; }
        public FeedbackRequest Feedback { get; set; }
    }
}
