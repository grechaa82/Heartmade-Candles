﻿using Bogus;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Tests.Admin.Core
{
    public class DecorTest
    {
        private static Faker _faker = new Faker("ru");

        [Theory, MemberData(nameof(GenerateData))]
        public void Create_ValidParameters_ReturnsSuccess(
            int id,
            string title,
            string description,
            decimal price,
            string imageURL,
            bool isActive)
        {
            // Arrange

            // Act
            var result = Decor.Create(
                title: title,
                description: description,
                price: price,
                imageURL: imageURL,
                isActive: isActive,
                id: id);

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
                    faker.Random.String(1, Decor.MaxTitleLenght),
                    faker.Random.String(1, Decor.MaxDescriptionLenght),
                    faker.Random.Number(1, 10000) * faker.Random.Decimal(),
                    faker.Image.PicsumUrl(),
                    faker.Random.Bool(),
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
            var result = Decor.Create(
                id: _faker.Random.Number(1, 10000),
                title: title,
                description: _faker.Random.String(1, Decor.MaxDescriptionLenght),
                price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
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
            var title = _faker.Random.String(Decor.MaxTitleLenght + 1);

            // Act
            var result = Make(title: title);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'title' cannot be more than {Decor.MaxTitleLenght} characters", result.Error);

        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_NullOrWhiteSpaceDescription_ShouldReturnFailure(string description)
        {
            // Arrange

            // Act
            var result = Decor.Create(
                id: _faker.Random.Number(1, 10000),
                title: _faker.Random.String(1, Decor.MaxTitleLenght),
                description: description,
                price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
                imageURL: _faker.Image.PicsumUrl(),
                isActive: _faker.Random.Bool());

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'description' cannot be null", result.Error);
        }

        [Fact]
        public void Create_LongDescription_ShouldReturnFailure()
        {
            // Arrange
            var description = _faker.Random.String(Decor.MaxDescriptionLenght + 1);

            // Act
            var result = Make(description: description);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'description' cannot be more than {Decor.MaxDescriptionLenght} characters", result.Error);

        }

        [Fact]
        public void Create_ZeroOrLessPrice_ShouldReturnFailure()
        {
            // Arrange
            var price = _faker.Random.Number(-10000, 0) * _faker.Random.Decimal();

            // Act
            var result = Make(price: price);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'price' сannot be 0 or less", result.Error);
        }

        [Fact]
        public void Create_InvalidParameters_ShouldReturnFailure()
        {
            // Arrange
            var title = "   ";
            string description = "";
            var price = -10.0m;
            var imageURL = "";

            // Act
            var result = Make(
                title: title,
                description: description,
                price: price,
                imageURL: imageURL);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'title' cannot be null or whitespace, 'description' cannot be null, 'price' сannot be 0 or less", result.Error);
        }

        private static Result<Decor> Make(
            int? id = null,
            string title = null,
            string description = null,
            decimal? price = null,
            string imageURL = null,
            bool? isActive = null)
        {
            var faker = new Faker();

            return Decor.Create(
                id: id ?? faker.Random.Number(1, 10000),
                title: title ?? faker.Random.String(1, Decor.MaxTitleLenght),
                description: description ?? faker.Random.String(1, Decor.MaxDescriptionLenght),
                price: price ?? faker.Random.Number(1, 10000) * faker.Random.Decimal(),
                imageURL: imageURL ?? faker.Image.PicsumUrl(),
                isActive: isActive ?? faker.Random.Bool()
            );
        }
    }
}