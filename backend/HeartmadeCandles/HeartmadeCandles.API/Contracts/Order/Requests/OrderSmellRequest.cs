using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class OrderSmellRequest
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public decimal Price { get; set; }
}
