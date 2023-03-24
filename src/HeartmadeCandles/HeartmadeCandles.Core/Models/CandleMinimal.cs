﻿namespace HeartmadeCandles.Core.Models
{
    public class CandleMinimal : ModelBase
    {
        private CandleMinimal(
            string id,
            string title,
            string imageURL,
            CandleType candleType,
            string description)
        {
            Id = id;
            Title = title;
            ImageURL = imageURL;
            CandleType = candleType;
            Description = description;
        }

        public string Title { get; }
        public string ImageURL { get; }
        public CandleType CandleType { get; }
        public string Description { get; }

        public static (CandleMinimal, ErrorDetail[]) Create(
            string title,
            string imageURL,
            CandleType candleType,
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

            if (candleType == null)
            {
                errorsMessage = $"'{nameof(title)}' connot be null.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var candleMinimal = new CandleMinimal(
                id,
                title,
                imageURL,
                candleType,
                description);

            return (candleMinimal, errors.ToArray());
        }
    }

    public enum CandleType
    {
        Shaped,
        Container
    }
}