namespace HeartmadeCandles.Order.Core.Models
{
    public class CandleDetailWithQuantityAndPrice
    {
        public CandleDetailWithQuantityAndPrice(CandleDetail candleDetail, int quantity)
        {
            CandleDetail = candleDetail;
            Quantity = quantity;
            Price = Math.Round(CalculatePrice() * quantity);
        }

        public CandleDetail CandleDetail { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

        private decimal CalculatePrice()
        {
            var decorPrice = CandleDetail.Decor != null ? CandleDetail.Decor.Price : 0; 
            var smellPrice = CandleDetail.Smell != null ? CandleDetail.Smell.Price : 0;

            var price = CandleDetail.Candle.Price + decorPrice + smellPrice + CandleDetail.Wick.Price;

            var gramsInLayer = CandleDetail.Candle.WeightGrams / CandleDetail.NumberOfLayer.Number;
            var layerColorsPrice = CandleDetail.LayerColors.Sum(layerColor => gramsInLayer * layerColor.PricePerGram);
            price += layerColorsPrice;

            return price;
        }

        public override string ToString()
        {
            string layerColors = string.Join(", ", CandleDetail.LayerColors.Select(lc => lc.Title));
            string decor = CandleDetail.Decor != null ? $"Decor={CandleDetail.Decor.Title}" : string.Empty;
            string smell= CandleDetail.Smell != null ? $"Smell={CandleDetail.Smell.Title}" : string.Empty;

            return $"Candle={CandleDetail.Candle.Title}, NumberOfLayer={CandleDetail.NumberOfLayer.Number}, " +
                   $"LayerColor={layerColors}" + (decor!= string.Empty ? $", {decor}" : string.Empty) +
                   (smell != string.Empty ? $", {smell}" : string.Empty) +
                   $", Wick={CandleDetail.Wick.Title}, Quantity={Quantity}";
        }
    }
}
