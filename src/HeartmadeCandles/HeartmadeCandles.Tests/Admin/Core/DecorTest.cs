using Bogus;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Tests.Admin.Core
{
    public class DecorTest
    {
        private static Faker _faker = new Faker();
        private readonly int _id;
        private readonly string _title;
        private readonly string _description;
        private readonly decimal _price;
        private readonly string _imageURL;
        private readonly bool _isActive;

        public DecorTest()
        {
            _id = _faker.Random.Number(1, 10000);
            _title = _faker.Random.String(1, Decor.MaxTitleLenght);
            _description = _faker.Random.String(1, Decor.MaxDescriptionLenght);
            _price = _faker.Random.Number(1, 10000) * _faker.Random.Decimal();
            _imageURL = _faker.Image.PicsumUrl();
            _isActive = _faker.Random.Bool();
        }

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
                var id = faker.Random.Number(1, 10000);
                var title = faker.Random.String(1, Decor.MaxTitleLenght);
                var description = faker.Random.String(1, Decor.MaxDescriptionLenght);
                var price = faker.Random.Number(1, 10000) * faker.Random.Decimal();
                var imageURL = faker.Image.PicsumUrl();
                var isActive = faker.Random.Bool();

                yield return new object[] {
                    id,
                    title,
                    description,
                    price,
                    imageURL,
                    isActive
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
            var title = _faker.Random.String(Decor.MaxTitleLenght + 1);

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

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_NullOrWhiteSpaceDescription_ShouldReturnFailure(string description)
        {
            // Arrange

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
            var description = _faker.Random.String(Decor.MaxDescriptionLenght + 1);

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
        public void Create_ZeroOrLessPrice_ShouldReturnFailure()
        {
            // Arrange
            var price = _faker.Random.Number(-10000, 0) * _faker.Random.Decimal();

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
