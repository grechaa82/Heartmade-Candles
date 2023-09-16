namespace HeartmadeCandles.API.Contracts.Requests
{
    public class CreateOrderRequest
    {
        public string ConfiguredCandlesString { get; set; }
        public OrderItemFilterRequest[] OrderItemFilters { get; set; }
        public UserRequest User { get; set; }
        public FeedbackRequest Feedback { get; set; }
    }
}
