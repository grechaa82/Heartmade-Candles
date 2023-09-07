using Bogus;
using Bogus.DataSets;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Tests.Admin.Core
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
                    new Image[] { Image.Create(faker.Random.String(1, Image.MaxAlternativeNameLenght), faker.Random.String()).Value },
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
                images: new Image[] { Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String()).Value },
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
            Assert.Equal($"'title' cannot be more than {LayerColor.MaxTitleLenght} characters", result.Error);

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
                id: _faker.Random.Number(1, 10000),
                title: _faker.Random.String(1, LayerColor.MaxTitleLenght),
                description: description,
                pricePerGram: _faker.Random.Number(1, 10000),
                images: new Image[] { Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String()).Value },
                isActive: _faker.Random.Bool());

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'description' cannot be null or whitespace", result.Error);
        }

        [Fact]
        public void Create_LongDescription_ShouldReturnFailure()
        {
            // Arrange
            var description = _faker.Random.String(LayerColor.MaxDescriptionLenght + 1);

            // Act
            var result = Make(description: description);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'description' cannot be more than {LayerColor.MaxDescriptionLenght} characters", result.Error);
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
            string description = "";
            var pricePerGram = -10.0m;
            var images = new Image[] { Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLenght), _faker.Random.String()).Value };

            // Act
            var result = Make(
            title: title,
                description: description,
                pricePerGram: pricePerGram,
                images: images);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'title' cannot be null or whitespace, 'description' cannot be null or whitespace, 'pricePerGram' cannot be 0 or less", result.Error);

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
                id: id ?? faker.Random.Number(1, 10000),
                title: title ??  faker.Random.String(1, LayerColor.MaxTitleLenght),
                description: description ?? faker.Random.String(1, LayerColor.MaxDescriptionLenght),
                pricePerGram: pricePerGram ??  faker.Random.Number(1, 10000),
                images: images ?? new Image[] { Image.Create(faker.Random.String(1, Image.MaxAlternativeNameLenght), faker.Random.String()).Value },
                isActive: isActive ?? faker.Random.Bool());
        }
    }
}