﻿namespace HeartmadeCandles.Core.Models
{
    public class Candle : ModelBase
    {
        private Candle(
            string id,
            bool isUsed,
            string title,
            string imageURL,
            List<int> numberOfLayers,
            List<LayerColor> layerColors,
            List<Smell> smells,
            List<Decor> decors,
            string description)
        {
            Id = id;
            IsUsed = isUsed;
            Title = title;
            ImageURL = imageURL;
            NumberOfLayers = numberOfLayers;
            LayerColors = layerColors;
            Smells = smells;
            Decors = decors;
            Description = description;
        }

        public bool IsUsed { get; }
        public string Title { get; }
        public string ImageURL { get; }
        public List<int> NumberOfLayers { get; }
        public List<LayerColor> LayerColors { get; }
        public List<Smell> Smells { get; }
        public List<Decor> Decors { get; }
        public string Description { get; }

        public static (Candle, ErrorDetail[]) Create(
            bool isUsed,
            string title,
            string imageURL,
            List<int> numberOfLayers,
            List<LayerColor> layerColors,
            List<Smell> smells,
            List<Decor> decors,
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

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var candle = new Candle(
                id,
                isUsed,
                title,
                imageURL,
                numberOfLayers,
                layerColors,
                smells,
                decors,
                description);

            return (candle, errors.ToArray());
        }
    }
}
