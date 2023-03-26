namespace HeartmadeCandles.Core.Models
{
    public class Smell : ModelBase
    {
        public Smell(
            string id,
            bool isUsed,
            string title,
            decimal price,
            string description)
        {
            Id = id;
            IsUsed = isUsed;
            Title = title;
            Price = price;
            Description = description;
        }

        public bool IsUsed { get; }
        public string Title { get; }
        public decimal Price { get; }
        public string Description { get; }

        public static (Smell, ErrorDetail[]) Create(
            bool isUsed,
            string title,
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

            var smell = new Smell(
                id,
                isUsed,
                title,
                price,
                description);

            return (smell, errors.ToArray());
        }
    }
}
