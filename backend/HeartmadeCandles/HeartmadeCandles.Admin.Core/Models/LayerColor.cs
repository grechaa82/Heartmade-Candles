using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models;

public class LayerColor
{
    public const int MaxTitleLenght = 48;
    public const int MaxDescriptionLenght = 256;

    private LayerColor(
        int id,
        string title,
        string description,
        decimal pricePerGram,
        Image[] images,
        bool isActive)
    {
        Id = id;
        Title = title;
        Description = description;
        PricePerGram = pricePerGram;
        Images = images;
        IsActive = isActive;
    }

    public int Id { get; }

    public string Title { get; }

    public string Description { get; }

    public decimal PricePerGram { get; }

    public Image[] Images { get; }

    public bool IsActive { get; }

    public static Result<LayerColor> Create(
        string title,
        string description,
        decimal pricePerGram,
        Image[] images,
        bool isActive = true,
        int id = 0)
    {
        var result = Result.Success();

        if (string.IsNullOrWhiteSpace(title))
        {
            result = Result.Combine(
                result,
                Result.Failure<LayerColor>($"'{nameof(title)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(title) && title.Length > MaxTitleLenght)
        {
            result = Result.Combine(
                result,
                Result.Failure<LayerColor>($"'{nameof(title)}' cannot be more than {MaxTitleLenght} characters"));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            result = Result.Combine(
                result,
                Result.Failure<LayerColor>($"'{nameof(description)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(description) && description.Length > MaxDescriptionLenght)
        {
            result = Result.Combine(
                result,
                Result.Failure<LayerColor>(
                    $"'{nameof(description)}' cannot be more than {MaxDescriptionLenght} characters"));
        }

        if (pricePerGram <= 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<LayerColor>($"'{nameof(pricePerGram)}' cannot be 0 or less"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<LayerColor>(result.Error);
        }

        var layerColor = new LayerColor(
            id,
            title,
            description,
            pricePerGram,
            images,
            isActive);

        return Result.Success(layerColor);
    }
}