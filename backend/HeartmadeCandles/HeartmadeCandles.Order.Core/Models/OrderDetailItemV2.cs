namespace HeartmadeCandles.Order.Core.Models;

public class OrderDetailItemV2
{
    public required Candle Candle { get; set; }
    public Decor? Decor { get; set; }
    public required LayerColor[] LayerColors { get; set; }
    public required NumberOfLayer NumberOfLayer { get; set; }
    public Smell? Smell { get; set; }
    public required Wick Wick { get; set; }
    public decimal Price => Math.Round(CalculatePrice() * Quantity);
    public int Quantity { get; set; }
    public required string ConfigurationString { get; set; }

    private decimal CalculatePrice()
    {
        var price = Candle.Price 
            + Decor?.Price ?? 0 
            + Smell?.Price ?? 0 
            + Wick.Price;
        var gramsInLayer = Candle.WeightGrams / NumberOfLayer.Number;

        var layerColorsPrice =
            LayerColors.Sum(l => l.CalculatePriceForGrams(gramsInLayer));

        price += layerColorsPrice;
        return price;
    }
}