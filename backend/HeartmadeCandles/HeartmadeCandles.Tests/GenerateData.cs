using Bogus;

namespace HeartmadeCandles.UnitTests;

internal class GenerateData
{
    private static readonly Faker _faker = new();

    public static int GenerateId() => _faker.Random.Number(1, 1_000_000);
    
    public static int GenerateInvalidId() => _faker.Random.Number(1_000_001, 2_000_000);

    public static string GenerateStringId() => Guid.NewGuid().ToString();
}
