using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Tests.Admin.Core.UnitTests
{
    public class CandleTests
    {
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
            _id = 1;
            _title = "Test candle";
            _description = "Test candle description";
            _price = 20;
            _weightGrams = 600;
            _imageURL = "https://testImageURL/candle.jpg";
            _isActive = true;
            _typeCandle = TypeCandle.OtherCandle;
            _createdAt = DateTime.UtcNow;
        }

        [Fact]
        public void CreateCandle_ValidParameters_ReturnsSuccess()
        {
            // Arrange

            // Act
            var result = Candle.Create(
                title: _title,
                description: _description,
                price: _price,
                weightGrams: _weightGrams,
                imageURL: _imageURL,
                isActive: _isActive,
                typeCandle: _typeCandle,
                id: _id,
                createdAt: _createdAt);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void CreateCandle_NullTitle_ShouldReturnFailure()
        {
            // Arrange
            string title = null;

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
            var title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

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

        [Fact]
        public void CreateCandle_NullDescription_ShouldReturnFailure()
        {
            // Arrange
            string description = null;

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
            var description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. " +
                              "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer " +
                              "took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries.";

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
        public void CreateCandle_ZeroPrice_ShouldReturnFailure()
        {
            // Arrange
            var price = 0m;

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
        public void CreateCandle_ZeroWeightGrams_ShouldReturnFailure()
        {
            // Arrange
            int weightGrams = 0;

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
    }
}