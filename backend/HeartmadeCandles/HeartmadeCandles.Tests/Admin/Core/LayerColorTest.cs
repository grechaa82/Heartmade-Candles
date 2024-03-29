﻿using Bogus;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.UnitTests.Admin.Core;

public class LayerColorTest
{
    private static readonly Faker _faker = new("ru");

    [Theory]
    [MemberData(nameof(GenerateTestDataForCreateValidParameters))]
    public void Create_ValidParameters_ReturnsSuccess(
        int id,
        string title,
        string description,
        decimal pricePerGram,
        Image[] images,
        bool isActive)
    {
        // Arrange

        // Act
        var result = LayerColor.Create(
            id: id,
            title: title,
            description: description,
            pricePerGram: pricePerGram,
            images: images,
            isActive: isActive);

        // Assert
        Assert.True(result.IsSuccess);
    }

    public static IEnumerable<object[]> GenerateTestDataForCreateValidParameters()
    {
        var faker = new Faker();

        for (var i = 0; i < 100; i++)
            yield return new object[]
            {
                GenerateData.GenerateId(),
                faker.Random.String(1, LayerColor.MaxTitleLength),
                faker.Random.String(1, LayerColor.MaxDescriptionLength),
                faker.Random.Number(1, 10000),
                new[]
                {
                    Image.Create(
                            faker.Random.String(1, Image.MaxAlternativeNameLength),
                            faker.Random.String())
                        .Value
                },
                faker.Random.Bool()
            };
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_NullOrWhiteSpaceTitle_ShouldReturnFailure(string title)
    {
        // Arrange

        // Act
        var result = LayerColor.Create(
            id: GenerateData.GenerateId(),
            title: title,
            description: _faker.Random.String(1, LayerColor.MaxDescriptionLength),
            pricePerGram: _faker.Random.Number(1, 10000),
            images: new[]
            {
                Image.Create(
                        _faker.Random.String(1, Image.MaxAlternativeNameLength),
                        _faker.Random.String())
                    .Value
            },
            isActive: _faker.Random.Bool());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("'title' cannot be null or whitespace", result.Error);
    }

    [Fact]
    public void Create_LongTitle_ShouldReturnFailure()
    {
        // Arrange
        var title = _faker.Random.String(Candle.MaxTitleLength + 1);

        // Act
        var result = Make(title: title);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"'title' cannot be more than {LayerColor.MaxTitleLength} characters", result.Error);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_NullOrWhiteSpaceDescription_ShouldReturnFailure(string description)
    {
        // Arrange

        // Act
        var result = LayerColor.Create(
            id: GenerateData.GenerateId(),
            title: _faker.Random.String(1, LayerColor.MaxTitleLength),
            description: description,
            pricePerGram: _faker.Random.Number(1, 10000),
            images: new[]
            {
                Image.Create(
                        _faker.Random.String(1, Image.MaxAlternativeNameLength),
                        _faker.Random.String())
                    .Value
            },
            isActive: _faker.Random.Bool());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("'description' cannot be null or whitespace", result.Error);
    }

    [Fact]
    public void Create_LongDescription_ShouldReturnFailure()
    {
        // Arrange
        var description = _faker.Random.String(LayerColor.MaxDescriptionLength + 1);

        // Act
        var result = Make(description: description);

        // Assert
        Assert.True(result.IsFailure);

        Assert.Equal(
            $"'description' cannot be more than {LayerColor.MaxDescriptionLength} characters",
            result.Error);
    }

    [Fact]
    public void Create_ZeroOrLessPricePerGram_ShouldReturnFailure()
    {
        // Arrange
        var price = _faker.Random.Number(-10000, 0) * _faker.Random.Decimal();

        // Act
        var result = Make(pricePerGram: price);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("'pricePerGram' cannot be 0 or less", result.Error);
    }

    [Fact]
    public void Create_InvalidParameters_ShouldReturnFailure()
    {
        // Arrange
        var title = "   ";
        var description = "";
        var pricePerGram = -10.0m;

        var images = new[]
        {
            Image.Create(
                    _faker.Random.String(1, Image.MaxAlternativeNameLength),
                    _faker.Random.String())
                .Value
        };

        // Act
        var result = Make(
            title: title,
            description: description,
            pricePerGram: pricePerGram,
            images: images);

        // Assert
        Assert.True(result.IsFailure);

        Assert.Equal(
            "'title' cannot be null or whitespace, 'description' cannot be null or whitespace, 'pricePerGram' cannot be 0 or less",
            result.Error);
    }

    private static Result<LayerColor> Make(
        int? id = null,
        string? title = null,
        string? description = null,
        decimal? pricePerGram = null,
        Image[]? images = null,
        bool? isActive = null)
    {
        var faker = new Faker();

        return LayerColor.Create(
            id: id ?? GenerateData.GenerateId(),
            title: title ?? faker.Random.String(1, LayerColor.MaxTitleLength),
            description: description ?? faker.Random.String(1, LayerColor.MaxDescriptionLength),
            pricePerGram: pricePerGram ?? faker.Random.Number(1, 10000),
            images: images
                    ?? new[]
                    {
                        Image.Create(
                                faker.Random.String(1, Image.MaxAlternativeNameLength),
                                faker.Random.String())
                            .Value
                    },
            isActive: isActive ?? faker.Random.Bool());
    }
}