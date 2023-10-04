using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class OrderItemFilterRequest
{
    [Required]
    public int CandleId { get; set; }
    public int DecorId { get; set; }
    [Required]
    public int NumberOfLayerId { get; set; }
    [Required]
    public int[] LayerColorIds { get; set; }
    public int SmellId { get; set; }
    [Required] 
    public int WickId { get; set; }
    [Required]
    public int Quantity { get; set; }
}