namespace HeartmadeCandles.Order.Core.Models;

public class OrderDetail
{
    public string Id { get; set; }

    public required OrderDetailItemV2[] Items { get; set; }

    public decimal TotalPrice => Items.Sum(x => x.Price);

    public int TotalQuantity => Items.Sum(x => x.Quantity);

    public string TotalConfigurationString => string.Join(".", Items.Select(x => x.ConfigurationString));
}

