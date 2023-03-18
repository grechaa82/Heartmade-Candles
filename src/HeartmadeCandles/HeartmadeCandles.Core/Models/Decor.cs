namespace HeartmadeCandles.Core.Models
{
    public class Decor : ModelBase
    {
        private Decor(
            string id,
            bool isUsed,
            string title,
            string imageURL,
            string description)
        {
            Id = id;
            IsUsed = isUsed;
            Title = title;
            ImageURL = imageURL;
            Description = description;
        }

        public bool IsUsed { get; }
        public string Title { get; }
        public string ImageURL { get; }
        public string Description { get; }

        public static (Decor, ErrorDetail[]) Create(
            bool isUsed,
            string title,
            string imageURL,
            string description,
            string id = null)
        {
            var errors = new List<ErrorDetail>();
            var errorsMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                errorsMessage = $"'{nameof(title)}' connot be null or whitespace.";
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
                description);

            return (decor, errors.ToArray());
        }
    }
}
