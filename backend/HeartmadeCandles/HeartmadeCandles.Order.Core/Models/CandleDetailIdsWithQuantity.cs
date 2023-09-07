namespace HeartmadeCandles.Order.Core.Models
{
    public class CandleDetailIdsWithQuantity
    {
        public CandleDetailIdsWithQuantity(
           int candleId,
           int decorId,
           int numberOfLayerId,
           int[] layerColorIds,
           int smellId,
           int wickId,
           int quantity)
        {
            CandleId = candleId;
            DecorId = decorId;
            NumberOfLayerId = numberOfLayerId;
            LayerColorIds = layerColorIds;
            SmellId = smellId;
            WickId = wickId;
            Quantity = quantity;
        }

        public int CandleId { get; private set; }
        public int DecorId { get; private set; }
        public int NumberOfLayerId { get; private set; }
        public int[] LayerColorIds { get; private set; }
        public int SmellId { get; private set; }
        public int WickId { get; private set; }
        public int Quantity { get; private set; }
    }
}
