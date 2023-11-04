namespace HeartmadeCandles.Order.DAL.Documents;


public class CandleDocument
{
    public int Id { get; set; }

    public string Title { get; set; }

    public decimal Price { get; set; }

    public int WeightGrams { get; set; }

    public ImageDocument[] Images { get; set; }
}