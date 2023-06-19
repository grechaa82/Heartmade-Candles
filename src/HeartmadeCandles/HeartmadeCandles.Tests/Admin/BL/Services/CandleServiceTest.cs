using Bogus;
using Bogus.DataSets;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using Moq;

namespace HeartmadeCandles.UnitTests.Admin.BL.Services
{
    public class CandleServiceTest
    {
        private static Faker _faker = new Faker();

        private readonly CandleService _service;
        
        private readonly Mock<ICandleRepository> _candleRepositoryMock = new Mock<ICandleRepository>();
        private readonly Mock<IDecorRepository> _decorRepositoryMock = new Mock<IDecorRepository>();
        private readonly Mock<ILayerColorRepository> _layerColorRepositoryMock = new Mock<ILayerColorRepository>();
        private readonly Mock<INumberOfLayerRepository> _numberOfLayerRepositoryMock = new Mock<INumberOfLayerRepository>();
        private readonly Mock<ISmellRepository> _smellRepositoryMock = new Mock<ISmellRepository>();
        private readonly Mock<ITypeCandleRepository> _typeCandleRepositoryMock = new Mock<ITypeCandleRepository>();
        private readonly Mock<IWickRepository> _wickRepositoryMock = new Mock<IWickRepository>();

        public CandleServiceTest()
        {
            _service = new CandleService(
                _candleRepositoryMock.Object, 
                _decorRepositoryMock.Object, 
                _layerColorRepositoryMock.Object, 
                _numberOfLayerRepositoryMock.Object,
                _smellRepositoryMock.Object,
                _typeCandleRepositoryMock.Object,
                _wickRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateDecor_WhenValid_ShouldReturnFailureAsync()
        {
            // Arrange
            var id = _faker.Random.Number(1, 100);
            var ids = new int[] { 1, 2, 3};

            var decors = new Decor[ids.Length];
            for ( int i = 0; i < ids.Length; i++ )
            {
                decors[i] = GenerateDecor(i);
            }

            _decorRepositoryMock.Setup(d => d.GetByIds(ids))
                .ReturnsAsync(decors)
                .Verifiable();

            _decorRepositoryMock.Setup(dr => dr.UpdateCandleDecor(id, decors))
                .Verifiable();

            // Act
            var result = await _service.UpdateDecor(id, ids);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ids.Length, decors.Length);
        }

        private static Decor GenerateDecor(int id = 0)
        {
            var decor = Decor.Create(
                   _faker.Random.String(1, Decor.MaxTitleLenght),
                   _faker.Random.String(1, Decor.MaxDescriptionLenght),
                   _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
                   _faker.Image.PicsumUrl(),
                   _faker.Random.Bool(),
                   id == 0 ? _faker.Random.Number(1, 10000) : id);

            return decor.Value;
        }

        [Fact]
        public async Task UpdateDecor_WhenAllDecorsNotExist_ShouldReturnFailureAsync()
        {
            // Arrange
            var id = _faker.Random.Number(1, 100);
            var ids = new int[] { 1, 2, 3 };

            _decorRepositoryMock.Setup(d => d.GetByIds(ids))
                .ReturnsAsync(Array.Empty<Decor>())
                .Verifiable();

            // Act
            var result = await _service.UpdateDecor(id, ids);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal($"'{string.Join(", ", ids)}' these ids do not exist", result.Error);
        }

        [Fact]
        public async Task UpdateDecor_WhenOneDecorNotExist_ShouldReturnFailureAsync()
        {
            // Arrange
            var id = _faker.Random.Number(1, 100);
            var ids = new int[] { 1, 2, 3, 4 };

            var decors = new Decor[ids.Length - 1];
            for (int i = 0; i < decors.Length; i++)
            {
                decors[ids[i] - 1] = GenerateDecor(ids[i]);
            }

            var idsToCheck = ids.Take(3).ToArray();

            _decorRepositoryMock.Setup(d => d.GetByIds(ids))
                .ReturnsAsync(decors)
                .Verifiable();

            // Act
            var result = await _service.UpdateDecor(id, ids);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("'4' these ids do not exist", result.Error);
        }

    }
}
