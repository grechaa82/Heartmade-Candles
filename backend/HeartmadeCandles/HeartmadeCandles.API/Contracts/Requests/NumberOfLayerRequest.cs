using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class NumberOfLayerRequest
{
    [Required] public int Number { get; set; }
}