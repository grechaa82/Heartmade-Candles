using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models;

public class NumberOfLayer
{
    private NumberOfLayer(int id, int number)
    {
        Id = id;
        Number = number;
    }

    public int Id { get; }

    public int Number { get; }

    public static Result<NumberOfLayer> Create(int number, int id = 0)
    {
        if (number <= 0 || number >= 32)
        {
            return Result.Failure<NumberOfLayer>($"'{nameof(number)}' must be in the range from 0 to 32");
        }

        var numberOfLayer = new NumberOfLayer(id, number);

        return Result.Success(numberOfLayer);
    }
}