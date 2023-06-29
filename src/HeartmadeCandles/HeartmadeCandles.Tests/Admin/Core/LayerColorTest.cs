﻿using Bogus;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.UnitTests.Admin.Core
{
    public class LayerColorTest
    {
        private static Faker _faker = new Faker("ru");

        [Theory, MemberData(nameof(GenerateData))]
        public void Create_ValidParameters_ReturnsSuccess(
            int id,
            string title,
            string description,
            decimal pricePerGram,
            string imageURL,
            bool isActive)
        {
            // Arrange

            // Act
            var result = LayerColor.Create(
                id: id,
                title: title,
                description: description,
                pricePerGram: pricePerGram,
                imageURL: imageURL,
                isActive: isActive);

            // Assert
            Assert.True(result.IsSuccess);
        }

        public static IEnumerable<object[]> GenerateData()
        {
            var faker = new Faker();

            for (int i = 0; i < 100; i++)
            {
                yield return new object[] {
                    faker.Random.Number(1, 10000),
                    faker.Random.String(1, LayerColor.MaxTitleLenght),
                    faker.Random.String(1, LayerColor.MaxDescriptionLenght),
                    faker.Random.Number(1, 10000),
                    faker.Image.PicsumUrl(),
                    faker.Random.Bool()
                };
            }
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
                id: _faker.Random.Number(1, 10000),
                title: title,
                description: _faker.Random.String(1, LayerColor.MaxDescriptionLenght),
                pricePerGram: _faker.Random.Number(1, 10000),
                imageURL: _faker.Image.PicsumUrl(),
                isActive: _faker.Random.Bool());

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'title' cannot be null or whitespace", result.Error);
        }

        [Fact]
        public void Create_LongTitle_ShouldReturnFailure()
        {
            // Arrange
            var title = _faker.Random.String(Candle.MaxTitleLenght + 1);

            // Act
            var result = Make(title: title);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'title' cannot be more than {LayerColor.MaxTitleLenght} characters", result.Error);

        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_NullOrWhiteSpaceDescription_ShouldReturnFailure(string description)
        {

        }

        [Fact]
        public void Create_LongDescription_ShouldReturnFailure()
        {

        }

        [Fact]
        public void Create_ZeroOrLessPricePerGram_ShouldReturnFailure()
        {

        }

        [Fact]
        public void Create_InvalidParameters_ShouldReturnFailure()
        {

        }

        private static Result<LayerColor> Make(
            int? id = null,
            string? title = null,
            string? description = null,
            decimal? pricePerGram = null,
            string? imageURL = null,
            bool? isActive = null)
        {
            var faker = new Faker();

            return LayerColor.Create(
                id: id ?? faker.Random.Number(1, 10000),
                title: title ??  faker.Random.String(1, LayerColor.MaxTitleLenght),
                description: description ?? faker.Random.String(1, LayerColor.MaxDescriptionLenght),
                pricePerGram: pricePerGram ??  faker.Random.Number(1, 10000),
                imageURL: imageURL ?? faker.Image.PicsumUrl(),
                isActive: isActive ?? faker.Random.Bool());
        }
    }
}