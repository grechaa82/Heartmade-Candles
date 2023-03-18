namespace HeartmadeCandles.Core.Models
{
    public class Smell : ModelBase
    {
        public Smell(
            string id,
            bool isUsed,
            string title)
        {
            Id = id;
            IsUsed = isUsed;
            Title = title;
        }

        public bool IsUsed { get; set; }

        public string Title { get; set; }

        public static (Smell, ErrorDetail[]) Create(
            bool isUsed,
            string title,
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

            var smell = new Smell(
                id,
                isUsed,
                title);

            return (smell, errors.ToArray());
        }
    }
}
