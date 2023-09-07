using Bogus;
using Bogus.DataSets;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Tests.Admin.Core
{
    public class CandleTests
    {
        private static Faker _faker = new Faker("ru");

        [Theory, MemberData(nameof(GenerateData))]
        public void Create_ValidParameters_ReturnsSuccess(
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
            // Arrange

            // Act
            var result = Candle.Create(
                title: title,
                description: description,
                price: price,
                weightGrams: weightGrams,
                images: images,
                isActive: isActive,
                typeCandle: typeCandle,
                id: id,
                createdAt: createdAt);

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
                    faker.Random.String(1, Candle.MaxTitleLength),
                    faker.Random.String(1, Candle.MaxDescriptionLength),
                    faker.Random.Number(1, 10000) * faker.Random.Decimal(),
                    faker.Random.Number(1, 10000),
                    new Image[] { Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String()).Value },
                    faker.Random.Bool(),
                    TypeCandle.Create(faker.Random.String(1, TypeCandle.MaxTitleLenght), faker.Random.Number(1, 10000)).Value,
                    faker.Date.Past(1)
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
            var result = Candle.Create(
                id: _faker.Random.Number(1, 10000),
                title: title,
                description: _faker.Random.String(1, Candle.MaxDescriptionLength),
                price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
                weightGrams: _faker.Random.Number(1, 10000),
                images: new Image[] { Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String()).Value },
                isActive: _faker.Random.Bool(),
                typeCandle: TypeCandle.Create(_faker.Random.String(1, TypeCandle.MaxTitleLenght), _faker.Random.Number(1, 10000)).Value,
                createdAt: _faker.Date.Past(1));

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
            Assert.Equal($"'title' cannot be more than {Candle.MaxTitleLength} characters", result.Error);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_NullOrWhiteSpaceDescription_ShouldReturnFailure(string description)
        {
            // Arrange

            // Act
            var result = Candle.Create(
                id: _faker.Random.Number(1, 10000),
                title: _faker.Random.String(1, Candle.MaxTitleLength),
                description: description,
                price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
                weightGrams: _faker.Random.Number(1, 10000),
                images: new Image[] { Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String()).Value },
                isActive: _faker.Random.Bool(),
                typeCandle: TypeCandle.Create(_faker.Random.String(1, TypeCandle.MaxTitleLenght), _faker.Random.Number(1, 10000)).Value,
                createdAt: _faker.Date.Past(1));

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'description' cannot be null or whitespace", result.Error);
        }

        [Fact]
        public void Create_LongDescription_ShouldReturnFailure()
        {
            // Arrange
            var description = _faker.Random.String(Candle.MaxDescriptionLength + 10);

            // Act
            var result = Make(description: description);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'description' cannot be more than {Candle.MaxDescriptionLength} characters", result.Error);
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
            Assert.Equal("'price' cannot be 0 or less", result.Error);
        }

        [Fact]
        public void Create_ZeroOrLessWeightGrams_ShouldReturnFailure()
        {
            // Arrange
            int weightGrams = _faker.Random.Number(-10000, 0);

            // Act
            var result = Make(weightGrams: weightGrams);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'weightGrams' cannot be 0 or less", result.Error);
        }

        [Fact]
        public void Create_NullTypeCandle_ShouldReturnFailure()
        {
            // Arrange
            TypeCandle typeCandle = null;

            // Act
            var result = Candle.Create(
                id: _faker.Random.Number(1, 10000),
                title: _faker.Random.String(1, Candle.MaxTitleLength),
                description: _faker.Random.String(1, Candle.MaxDescriptionLength),
                price: _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
                weightGrams: _faker.Random.Number(1, 10000),
                images: new Image[] { Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String()).Value },
                isActive: _faker.Random.Bool(),
                typeCandle: typeCandle,
                createdAt: _faker.Date.Past(1));

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'typeCandle' cannot be null", result.Error);
        }

        [Fact]
        public void Create_InvalidParameters_ShouldReturnFailure()
        {
            // Arrange
            var resultError = "'title' cannot be null or whitespace, " +
                "'description' cannot be more than 256 characters, " +
                "'price' cannot be 0 or less, " +
                "'weightGrams' cannot be 0 or less, " +
                "'typeCandle' cannot be null";

            TypeCandle typeCandle = null;

            // Act
            var result = Candle.Create(
                id: _faker.Random.Number(1, 10000),
                title: null,
                description: _faker.Random.String(Candle.MaxDescriptionLength + 1),
                price: -10m,
                weightGrams: 0,
                images: new Image[] { Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String()).Value },
                isActive: _faker.Random.Bool(),
                typeCandle: typeCandle,
                createdAt: _faker.Date.Past(1));

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(resultError, result.Error);
        }

        private static Result<Candle> Make(
           int? id = null,
           string? title = null,
           string? description = null,
           decimal? price = null,
           int? weightGrams = null,
           Image[]? images = null,
           bool? isActive = null,
           TypeCandle? typeCandle = null,
           DateTime? createdAt = null)
        {
            var faker = new Faker();

            return Candle.Create(
                id: id ?? faker.Random.Number(1, 10000),
                title: title ?? faker.Random.String(1, Candle.MaxTitleLength),
                description: description ?? faker.Random.String(1, Candle.MaxDescriptionLength),
                price: price ?? faker.Random.Number(1, 10000) * faker.Random.Decimal(),
                weightGrams: weightGrams ?? faker.Random.Number(1, 10000),
                images: images ?? new Image[] { Image.Create(faker.Random.String(1, Image.MaxAlternativeNameLenght), faker.Random.String()).Value },
                isActive: isActive ?? faker.Random.Bool(),
                typeCandle: typeCandle ?? TypeCandle.Create(faker.Random.String(1, TypeCandle.MaxTitleLenght), faker.Random.Number(1, 10000)).Value,
                createdAt: createdAt ?? faker.Date.Past(1));
        }
    }
}