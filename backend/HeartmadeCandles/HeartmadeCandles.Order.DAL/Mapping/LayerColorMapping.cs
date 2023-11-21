using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class LayerColorMapping
{
    public static LayerColor[] MapToLayerColors(LayerColorDocument[] layerColorsDocuments)
    {
        var layerColors= new List<LayerColor>();

        foreach (var layerColorDocument in layerColorsDocuments)
        {
            var layerColor= new LayerColor(
                layerColorDocument.Id,
                layerColorDocument.Title,
                layerColorDocument.PricePerGram
            );

            layerColors.Add(layerColor);
        }

        return layerColors.ToArray();
    }

    public static LayerColorDocument[] MapToLayerColorsDocument(LayerColor[] layerColors)
    {
        var layerColorDocuments = new List<LayerColorDocument>();

        foreach (var layerColor in layerColors)
        {
            var layerColorDocument = new LayerColorDocument
            {
                Id = layerColor.Id,
                Title = layerColor.Title,
                PricePerGram = layerColor.PricePerGram,
            };

            layerColorDocuments.Add(layerColorDocument);
        }

        return layerColorDocuments.ToArray();
    }
}
