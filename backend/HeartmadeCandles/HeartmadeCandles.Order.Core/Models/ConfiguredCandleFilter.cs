using System.Text;

namespace HeartmadeCandles.Order.Core.Models;

public class ConfiguredCandleFilter
{
    public int CandleId { get; set; }

    public int? DecorId { get; set; }

    public int NumberOfLayerId { get; set; }

    public required int[] LayerColorIds { get; set; }

    public int? SmellId { get; set; }

    public int WickId { get; set; }

    public int Quantity { get; set; }

    public string GetFilterString()
    {
        var filterString = new StringBuilder();

        filterString.Append("c-").Append(CandleId).Append("~");
        if (DecorId != null)
        {
            filterString.Append("d-").Append(DecorId).Append("~");
        }
        filterString.Append("n-").Append(NumberOfLayerId).Append("~");
        filterString.Append("l-").Append(string.Join("_", LayerColorIds)).Append("~");
        filterString.Append("s-").Append(SmellId).Append("~");
        filterString.Append("w-").Append(WickId).Append("~");
        filterString.Append("q-").Append(Quantity);

        return filterString.ToString();
    }
}