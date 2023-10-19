using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models;

public class Image
{
    public const int MaxAlternativeNameLength = 48;

    private Image(string fileName, string alternativeName)
    {
        FileName = fileName;
        AlternativeName = alternativeName;
    }

    public string FileName { get; }

    public string AlternativeName { get; }

    public static Result<Image> Create(string fileName, string alternativeName)
    {
        var result = Result.Success();

        if (string.IsNullOrWhiteSpace(fileName))
        {
            result = Result.Combine(
                result,
                Result.Failure<Image>($"'{nameof(fileName)}' cannot be null or whitespace"));
        }

        if (string.IsNullOrWhiteSpace(alternativeName))
        {
            result = Result.Combine(
                result,
                Result.Failure<Image>($"'{nameof(alternativeName)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(alternativeName) && alternativeName.Length > MaxAlternativeNameLength)
        {
            result = Result.Combine(
                result,
                Result.Failure<Image>(
                    $"'{nameof(alternativeName)}' cannot be more than {MaxAlternativeNameLength} characters"));
        }

        if (result.IsFailure)
        {
            Result.Failure<Image>(result.Error);
        }

        var image = new Image(fileName, alternativeName);

        return Result.Success(image);
    }
}