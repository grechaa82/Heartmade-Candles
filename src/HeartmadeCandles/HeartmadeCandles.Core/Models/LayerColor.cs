namespace HeartmadeCandles.Core.Models
{
    public class LayerColor : ModelBase
    {
        public LayerColor(
            string id,
            bool isUsed,
            string title,
            string hEX,
            string imageURL)
        {
            Id = id;
            IsUsed = isUsed;
            Title = title;
            HEX = hEX;
            ImageURL = imageURL;
        }

        public bool IsUsed { get; }
        public string Title { get; }
        public string HEX { get; }
        public string ImageURL { get; }

        public static (LayerColor, ErrorDetail[]) Create(
            bool isUsed,
            string title,
            string hex,
            string imageURL,
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

            var layerColor = new LayerColor(
                id,
                isUsed,
                title,
                hex,
                imageURL);

            return (layerColor, errors.ToArray());
        }

    }
}
