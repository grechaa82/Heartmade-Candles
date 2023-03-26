using System.Diagnostics;

namespace HeartmadeCandles.Core.Models
{
    public class LayerColor : ModelBase
    {
        public LayerColor(
            string id,
            bool isUsed,
            string title,
            string hEX,
            string imageURL,
            decimal pricePerGram,
            string description)
        {
            Id = id;
            IsUsed = isUsed;
            Title = title;
            HEX = hEX;
            ImageURL = imageURL;
            PricePerGram = pricePerGram;
            Description = description;
        }

        public bool IsUsed { get; }
        public string Title { get; }
        public string HEX { get; }
        public string ImageURL { get; }
        public decimal PricePerGram { get; }
        public string Description { get; }

        public static (LayerColor, ErrorDetail[]) Create(
            bool isUsed,
            string title,
            string hex,
            string imageURL,
            decimal pricePerGram,
            string id = null,
            string description = "")
        {
            var errors = new List<ErrorDetail>();
            var errorsMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                errorsMessage = $"'{nameof(title)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (pricePerGram <= 0)
            {
                errorsMessage = $"'{nameof(pricePerGram)}' сannot be 0 or less.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var layerColor = new LayerColor(
                id,
                isUsed,
                title,
                hex,
                imageURL,
                pricePerGram,
                description);

            return (layerColor, errors.ToArray());
        }

    }
}
