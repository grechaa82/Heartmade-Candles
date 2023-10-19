using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class ImageRequest
{
    [Required] public required string FileName { get; set; }

    [Required] public required string AlternativeName { get; set; }
}