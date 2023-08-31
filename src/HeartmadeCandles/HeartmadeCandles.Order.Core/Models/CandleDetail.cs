namespace HeartmadeCandles.Order.Core.Models
{
    public class CandleDetail
    {
        public CandleDetail(
            Candle candle, 
            Decor decor, 
            LayerColor[] layerColors, 
            NumberOfLayer numberOfLayer, 
            Smell smell, 
            Wick wick, 
            int quantity)
        {
            Candle = candle;
            Decor = decor;
            LayerColors = layerColors;
            NumberOfLayer = numberOfLayer;
            Smell = smell;
            Wick = wick;
            Quantity = quantity;
            Price = CalculatePrice();
        }

        public Candle Candle { get; private set; }
        public Decor Decor { get; private set; }
        public LayerColor[] LayerColors { get; private set; }
        public NumberOfLayer NumberOfLayer { get; private set; }
        public Smell Smell { get; private set; }
        public Wick Wick { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

        private decimal CalculatePrice()
        {
            var price = Candle.Price + Decor.Price + Smell.Price + Wick.Price;

            var gramsInLayer = Candle.WeightGrams / NumberOfLayer.Number;
            var layerColorsPrice = LayerColors.Sum(layerColor => gramsInLayer * layerColor.PricePerGram);
            price += layerColorsPrice;

            return Math.Round(price);
        }
    }
}
