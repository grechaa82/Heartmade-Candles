namespace HeartmadeCandles.Order.Core.Models;

public class Image
{
    public Image(string fileName, string alternativeName)
    {
        FileName = fileName;
        AlternativeName = alternativeName;
    }

    public string FileName { get; private set; }

    public string AlternativeName { get; private set; }
}