using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests;

public class ImageRequest
{
    [JsonConstructor]
    public ImageRequest(string fileName, string alternativeName)
    {
        FileName = fileName;
        AlternativeName = alternativeName;
    }

    public string FileName { get; set; }
    public string AlternativeName { get; set; }
}