using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;

namespace HeartmadeCandles.Admin.DAL.Mapping
{
    internal class LayerColorMapping
    {
        public static LayerColor MapToLayerColor(LayerColorEntity layerColorEntity)
        {
            var layerColor = LayerColor.Create(
                layerColorEntity.Title, 
                layerColorEntity.Description, 
                layerColorEntity.PricePerGram, 
                layerColorEntity.ImageURL, 
                layerColorEntity.IsActive, 
                layerColorEntity.Id);

            return layerColor.Value;
        }

        public static LayerColorEntity MapToLayerColorEntity(LayerColor layerColor)
        {
            var layerColorEntity = new LayerColorEntity()
            {
                Id = layerColor.Id,
                Title = layerColor.Title,
                Description = layerColor.Description,
                PricePerGram = layerColor.PricePerGram,
                ImageURL = layerColor.ImageURL,
                IsActive = layerColor.IsActive,
            };

            return layerColorEntity;
        }
    }
}
