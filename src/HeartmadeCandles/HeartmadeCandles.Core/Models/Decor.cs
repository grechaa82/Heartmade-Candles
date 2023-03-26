using System.Runtime.ConstrainedExecution;

namespace HeartmadeCandles.Core.Models
{
    public class Decor : ModelBase
    {
        private Decor(
            string id,
            bool isUsed,
            string title,
            string imageURL,
            decimal price,
            string description)
        {
            Id = id;
            IsUsed = isUsed;
            Title = title;
            ImageURL = imageURL;
            Price = price;
            Description = description;
        }

        public bool IsUsed { get; }
        public string Title { get; }
        public string ImageURL { get; }
        public decimal Price { get; }
        public string Description { get; }

        public static (Decor, ErrorDetail[]) Create(
            bool isUsed,
            string title,
            string imageURL,
            decimal price,
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

            if (price <= 0)
            {
                errorsMessage = $"'{nameof(price)}' сannot be 0 or less.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var decor = new Decor(
                id,
                isUsed,
                title,
                imageURL,
                price,
                description);

            return (decor, errors.ToArray());
        }
    }
}
