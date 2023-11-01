using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class LayerColorMapping
{
    public static LayerColor[] MapToLayerColors(LayerColorCollection[] layerColorsCollection)
    {
        var layerColors= new List<LayerColor>();

        foreach (var layerColorCollection in layerColorsCollection)
        {
            var layerColor= new LayerColor(
                layerColorCollection.Id,
                layerColorCollection.Title,
                layerColorCollection.PricePerGram
            );

            layerColors.Add(layerColor);
        }

        return layerColors.ToArray();
    }

    public static LayerColorCollection[] MapToLayerColorsCollection(LayerColor[] layerColors)
    {
        var layerColorsCollection = new List<LayerColorCollection>();

        foreach (var layerColor in layerColors)
        {
            var layerColorCollection = new LayerColorCollection
            {
                Id = layerColor.Id,
                Title = layerColor.Title,
                PricePerGram = layerColor.PricePerGram,
            };

            layerColorsCollection.Add(layerColorCollection);
        }

        return layerColorsCollection.ToArray();
    }
}
