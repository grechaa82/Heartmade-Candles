namespace HeartmadeCandles.API.Contracts.Requests
{
    public class OrderItemFilterRequest
    {
        public int CandleId { get; set; }
        public int DecorId { get; set; }
        public int NumberOfLayerId { get; set; }
        public int[] LayerColorIds { get; set; }
        public int SmellId { get; set; }
        public int WickId { get; set; }
        public int Quantity { get; set; }
    }
}