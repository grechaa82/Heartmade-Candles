using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class Image
    {
        public const int MaxAlternativeNameLenght = 48;

        private string _fileName;
        private string _alternativeName;

        private Image(string fileName, string alternativeName)
        {
            _fileName = fileName;
            _alternativeName = alternativeName;
        }

        public string FileName { get => _fileName; }
        public string AlternativeName { get => _alternativeName; }

        public static Result<Image> Create(string fileName, string alternativeName)
        {
            var result = Result.Success();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Image>($"'{nameof(fileName)}' cannot be null or whitespace"));
            }

            if (string.IsNullOrWhiteSpace(alternativeName))
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Image>($"'{nameof(alternativeName)}' cannot be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(alternativeName) && alternativeName.Length > MaxAlternativeNameLenght)
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Image>($"'{nameof(alternativeName)}' cannot be more than {MaxAlternativeNameLenght} characters"));
            }

            if (result.IsFailure)
            {
                Result.Failure<LayerColor>(result.Error);
            }

            var image = new Image(fileName, alternativeName);

            return Result.Success(image);
        }
    }
}