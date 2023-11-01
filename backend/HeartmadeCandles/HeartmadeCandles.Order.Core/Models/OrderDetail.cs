namespace HeartmadeCandles.Order.Core.Models;

public class OrderDetail
{
    public string? Id { get; set; }

    public required OrderDetailItem[] Items { get; set; }

    public decimal TotalPrice => Items.Sum(x => x.Price);

    public int TotalQuantity => Items.Sum(x => x.Quantity);

    // TODO: Think about whether there might be another `ConfigurationString`. It may be necessary to set this value and not bind to `string.Join(".", ...)`
    public string TotalConfigurationString => string.Join(".", Items.Select(x => x.ConfigurationString));
}
