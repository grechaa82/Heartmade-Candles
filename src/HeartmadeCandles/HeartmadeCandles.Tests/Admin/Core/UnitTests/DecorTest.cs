using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Tests.Admin.Core.UnitTests
{
    public class DecorTest
    {
        private readonly int _id;
        private readonly string _title;
        private readonly string _description;
        private readonly decimal _price;
        private readonly string _imageURL;
        private readonly bool _isActive;

        public DecorTest()
        {
            _id = 1;
            _title = "Test decor";
            _description = "Test decor description";
            _price = 10;
            _imageURL = "https://testImageURL/decor.jpg";
            _isActive = true;
        }

        [Fact]
        public void Create_ValidParameters_ReturnsSuccess()
        {
            // Arrange

            // Act
            var result = Decor.Create(
                id: _id,
                title: _title,
                description: _description,
                price: _price,
                imageURL: _imageURL,
                isActive: _isActive);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Create_NullTitle_ShouldReturnFailure()
        {
            // Arrange
            string title = null;

            // Act
            var result = Decor.Create(
                id: _id,
                title: title,
                description: _description,
                price: _price,
                imageURL: _imageURL,
                isActive: _isActive);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'title' connot be null or whitespace.", result.Error);
        }

        [Fact]
        public void Create_LongTitle_ShouldReturnFailure()
        {
            // Arrange
            var title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            // Act
            var result = Decor.Create(
                id: _id,
                title: title,
                description: _description,
                price: _price,
                imageURL: _imageURL,
                isActive: _isActive);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'title' connot be more than {Decor.MaxTitleLenght} characters.", result.Error);

        }

        [Fact]
        public void Create_NullDescription_ShouldReturnFailure()
        {
            // Arrange
            string description = null;

            // Act
            var result = Decor.Create(
                id: _id,
                title: _title,
                description: description,
                price: _price,
                imageURL: _imageURL,
                isActive: _isActive);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'description' connot be null.", result.Error);
        }

        [Fact]
        public void Create_LongDescription_ShouldReturnFailure()
        {
            // Arrange
            var description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. " +
                  "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer " +
                  "took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries.";

            // Act
            var result = Decor.Create(
                id: _id,
                title: _title,
                description: description,
                price: _price,
                imageURL: _imageURL,
                isActive: _isActive);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'description' connot be more than {Decor.MaxDescriptionLenght} characters.", result.Error);

        }

        [Fact]
        public void Create_ZeroPrice_ShouldReturnFailure()
        {
            // Arrange
            var price = 0m;

            // Act
            var result = Decor.Create(
                id: _id,
                title: _title,
                description: _description,
                price: price,
                imageURL: _imageURL,
                isActive: _isActive);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'price' сannot be 0 or less.", result.Error);
        }
    }
}
