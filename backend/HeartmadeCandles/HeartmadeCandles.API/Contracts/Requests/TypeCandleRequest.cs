using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class TypeCandleRequest
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
}