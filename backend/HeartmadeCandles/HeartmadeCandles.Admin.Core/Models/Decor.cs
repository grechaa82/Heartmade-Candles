﻿using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models;

public class Decor
{
    public const int MaxTitleLength = 48;
    public const int MaxDescriptionLength = 256;

    private Decor(
        int id,
        string title,
        string description,
        decimal price,
        Image[] images,
        bool isActive)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
        Images = images;
        IsActive = isActive;
    }

    public int Id { get; }

    public string Title { get; }

    public string Description { get; }

    public decimal Price { get; }

    public Image[] Images { get; }

    public bool IsActive { get; }

    public static Result<Decor> Create(
        string title,
        string description,
        decimal price,
        Image[] images,
        bool isActive = true,
        int id = 0)
    {
        var result = Result.Success();

        if (string.IsNullOrWhiteSpace(title))
        {
            result = Result.Combine(
                result,
                Result.Failure<Decor>($"'{nameof(title)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(title) && title.Length > MaxTitleLength)
        {
            result = Result.Combine(
                result,
                Result.Failure<Decor>($"'{nameof(title)}' cannot be more than {MaxTitleLength} characters"));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            result = Result.Combine(
                result,
                Result.Failure<Decor>($"'{nameof(description)}' cannot be null or whitespace"));
        }

        if (!string.IsNullOrWhiteSpace(description) && description.Length > MaxDescriptionLength)
        {
            result = Result.Combine(
                result,
                Result.Failure<Decor>(
                    $"'{nameof(description)}' cannot be more than {MaxDescriptionLength} characters"));
        }

        if (price <= 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<Decor>($"'{nameof(price)}' cannot be 0 or less"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<Decor>(result.Error);
        }

        var decor = new Decor(
            id,
            title,
            description,
            price,
            images,
            isActive);

        return Result.Success(decor);
    }
}