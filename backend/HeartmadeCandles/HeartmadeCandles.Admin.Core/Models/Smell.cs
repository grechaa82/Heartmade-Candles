using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models;

public class Smell
{
    public const int MaxTitleLenght = 48;
    public const int MaxDescriptionLenght = 256;

    private Smell(
        int id,
        string title,
        string description,
        decimal price,
        bool isActive)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
        IsActive = isActive;
    }

    public int Id { get; }

    public string Title { get; }

    public string Description { get; }

    public decimal Price { get; }

    public bool IsActive { get; }

    public static Result<Smell> Create(
        string title,
        string description,
        decimal price,
        bool isActive,
        int id = 0)
    {
        var result = Result.Success();

        if (string.IsNullOrWhiteSpace(title))
        {
            result = Result.Combine(
                result,
                Result.Failure<Smell>($"'{nameof(title)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(title) && title.Length > MaxTitleLenght)
        {
            result = Result.Combine(
                result,
                Result.Failure<Smell>($"'{nameof(title)}' cannot be more than {MaxTitleLenght} characters"));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            result = Result.Combine(
                result,
                Result.Failure<Smell>($"'{nameof(description)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(description) && description.Length > MaxDescriptionLenght)
        {
            result = Result.Combine(
                result,
                Result.Failure<Smell>(
                    $"'{nameof(description)}' cannot be more than {MaxDescriptionLenght} characters"));
        }

        if (price <= 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<Smell>($"'{nameof(price)}' сannot be 0 or less"));
        }

        var smell = new Smell(
            id,
            title,
            description,
            price,
            isActive);

        return Result.Success(smell);
    }
}