using Bogus;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Tests.Admin.Core
{
    public class CandleTests
    {
        private static Faker _faker = new Faker();
        private readonly int _id;
        private readonly string _title;
        private readonly string _description;
        private readonly decimal _price;
        private readonly int _weightGrams;
        private readonly string _imageURL;
        private readonly bool _isActive;
        private readonly TypeCandle _typeCandle;
        private readonly DateTime _createdAt;

        public CandleTests()
        {
            var typeCandle = TypeCandle.Create(_faker.Random.String(1, TypeCandle.MaxTitleLenght), _faker.Random.Number(1, 10000));

            _id = _faker.Random.Number(1, 10000);
            _title = _faker.Random.String(1, Candle.MaxTitleLenght);
            _description = _faker.Random.String(1, Candle.MaxDescriptionLenght);
            _price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal();
            _weightGrams = _faker.Random.Number(1, 10000);
            _imageURL = _faker.Image.PicsumUrl();
            _isActive = _faker.Random.Bool();
            _typeCandle = typeCandle.Value;
            _createdAt = _faker.Date.Past(1);
        }

        
        [Theory, MemberData(nameof(GenerateData))]
        public void CreateCandle_ValidParameters_ReturnsSuccess(
            int id,
            string title,
            string description,
            decimal price,
            int weightGrams,
            string imageURL,
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
                imageURL: imageURL,
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
            
            for (int i = 0; i < 1000; i++)
            {

                var id = faker.Random.Number(1, 10000);
                var title = faker.Random.String(1, Candle.MaxTitleLenght);
                var description = faker.Random.String(1, Candle.MaxDescriptionLenght);
                var price = faker.Random.Number(1, 10000) * faker.Random.Decimal();
                var weightGrams = faker.Random.Number(1, 10000);
                var imageURL = faker.Image.PicsumUrl();
                var isActive = faker.Random.Bool();
                var typeCandle = TypeCandle.Create(faker.Random.String(1, TypeCandle.MaxTitleLenght), _faker.Random.Number(1, 10000));
                var createdAt = faker.Date.Past(1);

                yield return new object[] {
                    id,
                    title,
                    description,
                    price,
                    weightGrams,
                    imageURL,
                    isActive,
                    typeCandle.Value,
                    createdAt
                };
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CreateCandle_NullOrWhiteSpaceTitle_ShouldReturnFailure(string title)
        {
            // Arrange

            // Act
            var result = Candle.Create(
                title: title,
                description: _description,
                price: _price,
                weightGrams: _weightGrams,
                imageURL: _imageURL,
                isActive: _isActive,
                typeCandle: _typeCandle,
                id: _id,
                createdAt: _createdAt);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'title' connot be null or whitespace.", result.Error);
        }

        [Fact]
        public void CreateCandle_LongTitle_ShouldReturnFailure()
        {
            // Arrange
            var title = _faker.Random.String(Candle.MaxTitleLenght + 1);

            // Act
            var result = Candle.Create(
                title: title,
                description: _description,
                price: _price,
                weightGrams: _weightGrams,
                imageURL: _imageURL,
                isActive: _isActive,
                typeCandle: _typeCandle,
                id: _id,
                createdAt: _createdAt);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'title' connot be more than {Candle.MaxTitleLenght} characters.", result.Error);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CreateCandle_NullOrWhiteSpaceDescription_ShouldReturnFailure(string description)
        {
            // Arrange

            // Act
            var result = Candle.Create(
                title: _title,
                description: description,
                price: _price,
                weightGrams: _weightGrams,
                imageURL: _imageURL,
                isActive: _isActive,
                typeCandle: _typeCandle,
                id: _id,
                createdAt: _createdAt);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'description' connot be null.", result.Error);
        }

        [Fact]
        public void CreateCandle_LongDescription_ShouldReturnFailure()
        {
            // Arrange
            var description = _faker.Random.String(Candle.MaxDescriptionLenght + 1);

            // Act
            var result = Candle.Create(
                title: _title,
                description: description,
                price: _price,
                weightGrams: _weightGrams,
                imageURL: _imageURL,
                isActive: _isActive,
                typeCandle: _typeCandle,
                id: _id,
                createdAt: _createdAt);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'description' connot be more than {Candle.MaxDescriptionLenght} characters.", result.Error);
        }

        [Fact]
        public void CreateCandle_ZeroOrLessPrice_ShouldReturnFailure()
        {
            // Arrange
            var price = _faker.Random.Number(-10000, 0) * _faker.Random.Decimal();

            // Act
            var result = Candle.Create(
                title: _title,
                description: _description,
                price: price,
                weightGrams: _weightGrams,
                imageURL: _imageURL,
                isActive: _isActive,
                typeCandle: _typeCandle,
                id: _id,
                createdAt: _createdAt);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'price' сannot be 0 or less.", result.Error);
        }

        [Fact]
        public void CreateCandle_ZeroOrLessWeightGrams_ShouldReturnFailure()
        {
            // Arrange
            int weightGrams = _faker.Random.Number(-10000, 0);

            // Act
            var result = Candle.Create(
                title: _title,
                description: _description,
                price: _price,
                weightGrams: weightGrams,
                imageURL: _imageURL,
                isActive: _isActive,
                typeCandle: _typeCandle,
                id: _id,
                createdAt: _createdAt);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'weightGrams' сannot be 0 or less.", result.Error);
        }

        [Fact]
        public void CreateCandle_NullTypeCandle_ShouldReturnFailure()
        {
            // Arrange
            TypeCandle typeCandle = null;

            // Act
            var result = Candle.Create(
                title: _title,
                description: _description,
                price: _price,
                weightGrams: _weightGrams,
                imageURL: _imageURL,
                isActive: _isActive,
                typeCandle: typeCandle,
                id: _id,
                createdAt: _createdAt);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'typeCandle' connot be null.", result.Error);
        }
    }
}