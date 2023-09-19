using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models;

public class Candle
{
    public const int MaxTitleLength = 48;
    public const int MaxDescriptionLength = 256;

    private Candle(
        int id,
        string title,
        string description,
        decimal price,
        int weightGrams,
        Image[] images,
        bool isActive,
        TypeCandle typeCandle,
        DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
        WeightGrams = weightGrams;
        Images = images;
        IsActive = isActive;
        TypeCandle = typeCandle;
        CreatedAt = createdAt;
    }

    public int Id { get; }

    public string Title { get; }

    public string Description { get; }

    public decimal Price { get; }

    public int WeightGrams { get; }

    public Image[] Images { get; }

    public bool IsActive { get; }

    public TypeCandle TypeCandle { get; }

    public DateTime CreatedAt { get; }

    public static Result<Candle> Create(
        string title,
        string description,
        decimal price,
        int weightGrams,
        Image[] images,
        TypeCandle typeCandle,
        bool isActive = true,
        int id = 0,
        DateTime? createdAt = null)
    {
        var result = Result.Success();

        if (string.IsNullOrWhiteSpace(title))
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(title)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(title) && title.Length > MaxTitleLength)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(title)}' cannot be more than {MaxTitleLength} characters"));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(description)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(description) && description.Length > MaxDescriptionLength)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>(
                    $"'{nameof(description)}' cannot be more than {MaxDescriptionLength} characters"));
        }

        if (price <= 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(price)}' cannot be 0 or less"));
        }

        if (weightGrams <= 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(weightGrams)}' cannot be 0 or less"));
        }

        if (typeCandle == null)
        {
            result = Result.Combine(
                result,
                Result.Failure<Candle>($"'{nameof(typeCandle)}' cannot be null"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<Candle>(result.Error);
        }

        var candle = new Candle(
            id,
            title,
            description,
            price,
            weightGrams,
            images,
            isActive,
            typeCandle,
            createdAt ?? DateTime.UtcNow);

        return Result.Success(candle);
    }
}