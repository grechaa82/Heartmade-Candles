using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class TypeCandle
    {
        public const int MaxTitleLenght = 48;

        private int _id;
        private string _title;

        private TypeCandle(int id, string title)
        {
            _id = id;
            _title = title;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }

        public static Result<TypeCandle> Create(string title, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result.Failure<TypeCandle>($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (title.Length > MaxTitleLenght)
            {
                return Result.Failure<TypeCandle>($"'{nameof(title)}' connot be more than {MaxTitleLenght} characters.");
            }

            var typeCandle = new TypeCandle(id, title);

            return Result.Success(typeCandle);
        }
    }
}
