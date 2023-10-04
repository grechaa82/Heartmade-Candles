using System.ComponentModel.DataAnnotations;
namespace HeartmadeCandles.API.Contracts.Requests;

public class ImageRequest
{
    [Required]
    public string FileName { get; set; }
    [Required]
    public string AlternativeName { get; set; }
}