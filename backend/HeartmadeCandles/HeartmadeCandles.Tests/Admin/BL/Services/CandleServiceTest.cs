using Bogus;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using Moq;

namespace HeartmadeCandles.UnitTests.Admin.BL.Services;

public class CandleServiceTest
{
    private static readonly Faker _faker = new();

    private readonly Mock<ICandleRepository> _candleRepositoryMock = new(MockBehavior.Strict);
    private readonly Mock<IDecorRepository> _decorRepositoryMock = new(MockBehavior.Strict);
    private readonly Mock<ILayerColorRepository> _layerColorRepositoryMock = new(MockBehavior.Strict);
    private readonly Mock<INumberOfLayerRepository> _numberOfLayerRepositoryMock = new(MockBehavior.Strict);

    private readonly CandleService _service;
    private readonly Mock<ISmellRepository> _smellRepositoryMock = new(MockBehavior.Strict);
    private readonly Mock<ITypeCandleRepository> _typeCandleRepositoryMock = new(MockBehavior.Strict);
    private readonly Mock<IWickRepository> _wickRepositoryMock = new(MockBehavior.Strict);

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
        var ids = new[] { 1, 2, 3 };

        var decors = new Decor[ids.Length];
        for (var i = 0; i < ids.Length; i++) decors[i] = GenerateDecor(i);

        _decorRepositoryMock.Setup(d => d.GetByIds(ids))
            .ReturnsAsync(decors)
            .Verifiable();

        _decorRepositoryMock.Setup(dr => dr.UpdateCandleDecor(id, decors))
            .ReturnsAsync(Result.Success)
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
            _faker.Random.String(1, Decor.MaxTitleLength),
            _faker.Random.String(1, Decor.MaxDescriptionLength),
            _faker.Random.Number(1, 10000) * _faker.Random.Decimal(),
            new[]
            {
                Image.Create(_faker.Random.String(1, Image.MaxAlternativeNameLength), _faker.Random.String()).Value
            },
            _faker.Random.Bool(),
            id == 0 ? _faker.Random.Number(1, 10000) : id);

        return decor.Value;
    }

    [Fact]
    public async Task UpdateDecor_WhenAllDecorsNotExist_ShouldReturnFailureAsync()
    {
        // Arrange
        var id = _faker.Random.Number(1, 100);
        var ids = new[] { 1, 2, 3 };

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
        var ids = new[] { 1, 2, 3, 4 };

        var decors = new Decor[ids.Length - 1];
        for (var i = 0; i < decors.Length; i++) decors[ids[i] - 1] = GenerateDecor(ids[i]);

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